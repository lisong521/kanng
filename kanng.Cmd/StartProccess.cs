﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;

namespace kanng.Cmd
{
    public partial class StartProccess : Form
    {
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 加载的文件路径
        /// </summary>
        public string LoadPath
        {
            get;
            set;
        }

        public int Limetime
        {
            get;
            set;
        }

        /// <summary>
        /// 文件扩展名 类型 文件 可以分为  可以执行文件 和 非 执行文件  
        /// </summary>
        int pathAttr = 0;

        public StartProccess(string path)
        {
            LoadPath = path;

            Title = "看门狗桌面助手1.4-" + path;

            InitializeComponent();

            richTextBox1.AllowDrop = true;

            richTextBox1.DragEnter += new DragEventHandler(richTextBox1_DragEnter);

            richTextBox1.DragDrop += new DragEventHandler(richTextBox1_DragDrop);
        }

        private void richTextBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            Array arrarFileName = (Array)e.Data.GetData(DataFormats.FileDrop);
            string strFileName = arrarFileName.GetValue(0).ToString();


            richTextBox1.Text += strFileName + "\r\n";

            KanngHelper.SingleKanng(LoadPath).WriteFile(richTextBox1.Text);


        }

        private void button2_Click(object sender, EventArgs e)
        {

            string[] strs = KanngHelper.SingleKanng(LoadPath).ReadAllLines();
            bool rlb = false;
            string errorFiles = "";
            if (strs != null)
            {
                foreach (string str in strs)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            //跳过注释
                            if (!str.StartsWith("#"))
                            {

                                rlb |= true;
                                Process.Start(str);
                                Thread.Sleep(Limetime);

                                //if (File.Exists(str) || Directory.Exists(str))
                                //{

                                //}
                                ////else if (Directory.Exists(str))
                                ////{
                                ////    rlb |= true;
                                ////    Process.Start("Explorer.exe", str);
                                ////    Thread.Sleep(Limetime);
                                ////}
                                //else
                                //{
                                //    errorFiles += str + "\r\n";
                                //}
                            }
                        }
                    }
                    catch (Exception ex)//容错处理
                    {
                        errorFiles += str + "\r\n";
                    }
                }
            }

            if (!rlb)
            {
                MessageBox.Show("没有需要启动的文件,您可以拖动文件到界面来添加文件!");
            }
            if (errorFiles.Length > 0)
            {
                MessageBox.Show("没有启动成功的文件:\r\n" + errorFiles);
            }
        }

        private void StartProccess_Load(object sender, EventArgs e)
        {
            this.Text = Title;
            Limetime = 1000;
            // LoadUrl();  
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //richTextBox1.Text = KanngHelper.ReadAllText();

        }

        private void tabPage1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void tabPage1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            if (s != null)
            {
                for (i = 0; i < s.Length; i++)
                {
                    richTextBox1.Text += s[i] + "\r\n";
                }
            }
            KanngHelper.SingleKanng(LoadPath).WriteFile(richTextBox1.Text);
        }


        private void 关于看门狗ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about aboutform = new about();
            aboutform.Show();
        }

        private void 赞助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jz aboutform = new jz();
            aboutform.Show();
        }
        private UrlPatte urlPatte1 = null;

        public void LoadUrl()
        {

            // Uri rulpaht = new Uri("http://www.kanng.net");

            //webBrowser1.Url = rulpaht;

            richTextBox1.Text = KanngHelper.SingleKanng(LoadPath).ReadAllText();


            List<UrlModel> urlModel = UrlXmlIO.ReadAllUrl();
            if (urlModel != null)
            {
                foreach (UrlModel model in urlModel)
                {
                    this.urlPatte1 = new UrlPatte();

                    urlPatte1.SetModel(model.guid, model.url, model.name);
                    // 
                    // urlPatte1
                    // 
                    this.urlPatte1.Cursor = System.Windows.Forms.Cursors.Hand;
                    this.urlPatte1.Location = new System.Drawing.Point(3, 3);
                    this.urlPatte1.Name = "urlPatte1";
                    this.urlPatte1.Size = new System.Drawing.Size(87, 51);
                    this.urlPatte1.TabIndex = 3;

                    this.flowLayoutPanel1.Controls.Add(this.urlPatte1);

                }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            CreatUrl create = new CreatUrl();
            create.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.kanng.net");
        }

        public void addControl(UrlModel model)
        {
            //this.Refresh();
            this.urlPatte1 = new UrlPatte();

            urlPatte1.SetModel(model.guid, model.url, model.name);
            // 
            // urlPatte1
            // 
            this.urlPatte1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.urlPatte1.Location = new System.Drawing.Point(3, 3);
            this.urlPatte1.Name = "urlPatte1";
            this.urlPatte1.Size = new System.Drawing.Size(87, 51);
            this.urlPatte1.TabIndex = 3;
            this.flowLayoutPanel1.Controls.Add(this.urlPatte1);

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControl1.SelectedIndex == 2)
            {
                this.WindowState = FormWindowState.Maximized;

                richTextBox2.Text = File.ReadAllText(@"C:\Windows\System32\drivers\etc\hosts");
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<UrlModel> urlModel = UrlXmlIO.ReadAllUrl();
            if (urlModel != null)
            {
                foreach (UrlModel model in urlModel)
                {
                    System.Diagnostics.Process.Start(model.url);

                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mspaint.exe");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("regedit.exe");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.S)
            {
                KanngHelper.SingleKanng(LoadPath).WriteFile(richTextBox1.Text);
            }
        }


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void iT导航ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.kanng.net");
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 3)
            {
                KanngHelper.SingleKanng(LoadPath).WriteHostsFile(richTextBox2.Text);
            }
            else
            {
                KanngHelper.SingleKanng(LoadPath).WriteFile(richTextBox1.Text);
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProject newp = new NewProject();
            newp.Show();
        }

    }





}
