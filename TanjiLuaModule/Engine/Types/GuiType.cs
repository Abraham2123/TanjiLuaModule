﻿using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TanjiLuaModule.Engine.Types
{
    internal class GuiType
    {
        private Form Form;
        private readonly ScriptProcess _scriptProcess;

        public GuiType(MainForm mainForm, ScriptProcess scriptp)
        {
            _scriptProcess = scriptp;      
        }

        public void Create(string title, int width, int height)
        {
            Form = new Form();
            Form.Disposed += new EventHandler(delegate
            {
                _scriptProcess.Dispose();
            });
            var resources = new ComponentResourceManager(typeof(MainForm));
            Form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Form.Width = width;
            Form.Text = title;
            Form.Height = height;
            Form.Icon = ((Icon)(resources.GetObject("$this.Icon")));
        }

        public void TopMost(bool active)
        {
            Form.TopMost = active;
        }

        public void AddButton(string id, string text, int x, int y)
        {
            var script = _scriptProcess.Script;
            var button = new Button
            {
                Name = id,
                Text = text,
                Location = new Point(x, y)
            };
            button.Click += new EventHandler(delegate {
                try
                {
                    script.Call(script.Globals["button_"+id+"_click"]);
                }
                catch (Exception)
                {
                    // ignored
                }
            });
            Form.Controls.Add(button);
        }


        public void AddLabel(string id, string text, int x, int y)
        {
            var script = _scriptProcess.Script;
            var label = new Label
            {
                Name = id,
                Text = text,
                Location = new Point(x, y)
            };
            Form.Controls.Add(label);
        }

        public void AddCheckBox(string id, string text, int x, int y)
        {
            var script = _scriptProcess.Script;
            var checkbox = new CheckBox
            {
                Name = id,
                Text = text,
                Location = new Point(x, y)
            };
            checkbox.Click += new EventHandler(delegate {
                try
                {
                    script.Call(script.Globals["checkbox_" + id + "_click"], checkbox.Checked);
                } catch (Exception) { }
            });
            Form.Controls.Add(checkbox);

        }

        public void AddTextbox(string id, int x, int y)
        {
            var script = _scriptProcess.Script;
            var textbox = new TextBox()
            {
                Name = id,
                Location = new Point(x, y)
            };
            Form.Controls.Add(textbox);
        }

        public string GetValue(string name)
        {
            if (!Form.Controls.ContainsKey(name))
            {
                return null;
            }
            var ctns = Form.Controls.Find(name, true);
            var ctn = ctns.GetValue(0) as Control;
            return ctn?.Text;
        }

        public void SetValue(string name, string value)
        {
            if (!Form.Controls.ContainsKey(name))
            {
                return;
            }
            var ctns = Form.Controls.Find(name, true);
            var ctn = ctns.GetValue(0) as Control;
            if (ctn != null) ctn.Text = value;
        }

        public bool IsChecked(string ckbox)
        {
            if (!Form.Controls.ContainsKey(ckbox))
            {
                return false;
            }
            var ctns = Form.Controls.Find(ckbox, true);
            var ctn = ctns.GetValue(0) as Control;
            
            if (!(ctn is CheckBox)) return false;
            
            var ckb = ctn as CheckBox;
            return ckb.Checked;
        }

        public void Show()
        {
            Form.Show();
        }
    }
}
