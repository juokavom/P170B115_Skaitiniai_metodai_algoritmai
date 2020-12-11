﻿//Jokubas Akramas IFF-8/12 7 var.
//P170B115 Skaitiniai metodai ir algoritmai (6 kr.)
//IV projektinė užduotis

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pvz1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        Series z1a, z1b, z2a, z2b, p1a, p1b, p2a, p2b, p3a, p3b;
        private static int m1, m2, tg;
        private static double k1, k2, ha, hb, va, vb, g, ka, kb, m, ta, tb, step;
        private static string line = new string('-', 94);
        bool aDone, bDone;
        private void pradinesReiksmes(double local_step)
        {
            m1 = 70;
            m2 = 15;
            tg = 40;
            k1 = 0.1;
            k2 = 5;
            ha = 4000;
            hb = 4000;
            va = 0;
            vb = 0;
            g = 9.8;
            ka = k1;
            kb = k1;
            m = m1 + m2;
            ta = 0;
            tb = 0;
            step = local_step;
            aDone = false;
            bDone = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm1();
            step = (double)this.numericUpDown1.Value;
            PreparareForm(chart1, -10, 120, -10, 4500);
            PreparareForm(chart2, -10, 120, -10, 100);
            //---
            richTextBox1.AppendText("Paprastųjų diferencialinių lygčių sprendimas (7var).\n");
            richTextBox1.AppendText(line + "\n");
            //---
            z1a = chart1.Series.Add("Aukštis h (m), žingsnis = step");
            z1a.ChartType = SeriesChartType.Line;
            z1a.Color = Color.Blue;
            //---
            z1b = chart1.Series.Add("Aukštis h (m), žingsnis = step/2");
            z1b.ChartType = SeriesChartType.Line;
            z1b.Color = Color.Green;
            //---
            z2a = chart2.Series.Add("Greitis v (m/s), žingsnis = step");
            z2a.ChartType = SeriesChartType.Line;
            z2a.Color = Color.Blue;
            //---
            z2b = chart2.Series.Add("Greitis v (m/s), žingsnis = step/2");
            z2b.ChartType = SeriesChartType.Line;
            z2b.Color = Color.Green;
            //---
            p1a = chart1.Series.Add("Iššokimas iš lėktuvo");
            p1a.ChartType = SeriesChartType.Point;
            p1a.Color = Color.Black;
            //---
            p1b = chart2.Series.Add("Iššokimas iš lėktuvo");
            p1b.ChartType = SeriesChartType.Point;
            p1b.Color = Color.Black;
            //---
            p2a = chart1.Series.Add("Parašiuto išskleidimas");
            p2a.ChartType = SeriesChartType.Point;
            p2a.Color = Color.Red;
            //---
            p2b = chart2.Series.Add("Parašiuto išskleidimas");
            p2b.ChartType = SeriesChartType.Point;
            p2b.Color = Color.Red;
            //---
            p3a = chart1.Series.Add("Nusileidimas ant žemės");
            p3a.ChartType = SeriesChartType.Point;
            p3a.Color = Color.DeepPink;
            //---
            p3b = chart2.Series.Add("Nusileidimas ant žemės");
            p3b.ChartType = SeriesChartType.Point;
            p3b.Color = Color.DeepPink;
            //---
            if (radioButton3.Checked) richTextBox1.AppendText("Sprendžiama Eulerio metodu\n");
            else if (radioButton4.Checked) richTextBox1.AppendText("Sprendžiama 4 eilės Rungės ir Kutos metodu\n");
            richTextBox1.AppendText(string.Format("step = {0}\n", step));
            richTextBox1.AppendText(string.Format("(step)   Iššokta iš lėktuvo, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", ha, ta, va));
            richTextBox1.AppendText(string.Format("(step/2) Iššokta iš lėktuvo, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", hb, tb, vb));
            //---
            z1a.Points.AddXY(ta, ha);
            z2a.Points.AddXY(ta, va);
            z1b.Points.AddXY(tb, hb);
            z2b.Points.AddXY(tb, vb);
            p1a.Points.AddXY(ta, ha);
            p1a.Points.AddXY(tb, hb);
            p1b.Points.AddXY(ta, va);
            p1b.Points.AddXY(tb, vb);
            //---
            z1a.BorderWidth = 1;
            z2a.BorderWidth = 1;
            z1b.BorderWidth = 1;
            z2b.BorderWidth = 1;
            p1a.BorderWidth = 3;
            p1b.BorderWidth = 3;
            p2a.BorderWidth = 3;
            p2b.BorderWidth = 3;
            p3a.BorderWidth = 3;
            p3b.BorderWidth = 3;
            //---
            timer1.Enabled = true;
            timer1.Interval = 1;
            timer1.Start();
            //---
        }
        private void rodytiGrafika(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                chart2.Visible = false;
                chart1.Visible = true;
            }
            else if (radioButton2.Checked)
            {
                chart1.Visible = false;
                chart2.Visible = true;
            }
        }

        private double f(double v_local, double k_local)
        {
            return g - ((k_local * Math.Pow(v_local, 2)) / m);
        }
        private double Eulerio(double v_local, double step_local, double k_local)
        {
            return v_local + step_local * f(v_local, k_local);
        }
        private double RK(double v_local, double step_local, double k_local)
        {
            double v1 = v_local + (step_local / 2) * f(v_local, k_local);
            double v2 = v_local + (step_local / 2) * f(v1, k_local);
            double v3 = v_local + step_local * f(v2, k_local);
            double v4 = v_local + (step_local / 6) * (f(v_local, k_local) + 2 * f(v1, k_local) + 2 * f(v2, k_local) + f(v3, k_local));
            return v4;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //---
            if (ta >= tg && ka == k1)
            {
                ka = k2;
                p2a.Points.AddXY(ta, ha);
                p2b.Points.AddXY(ta, va);
                richTextBox1.AppendText(string.Format("(step)   Išskleistas parašiutas, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", ha, ta, va));
            }
            //---
            if (tb >= tg && kb == k1)
            {
                kb = k2;
                p2b.Points.AddXY(tb, vb);
                p2b.Points.AddXY(tb, vb);
                richTextBox1.AppendText(string.Format("(step/2) Išskleistas parašiutas, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", hb, tb, vb));
            }
            //---
            ha -= step * (va);
            hb -= step/2 * (vb);
            if (radioButton3.Checked)
            {
                va = Eulerio(va, step, ka);
                vb = Eulerio(vb, step / 2, kb);
            }
            else if (radioButton4.Checked)
            {
                va = RK(va, step, ka);
                vb = RK(vb, step / 2, kb);
            }
            ta += step;
            tb += step/2;
            //---
            if (ha > 0)
            {
                z1a.Points.AddXY(ta, ha);
                z2a.Points.AddXY(ta, va);
            }
            else if (!aDone)
            {
                richTextBox1.AppendText(string.Format("(step)   Nusileidimas ant žemės, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", ha, ta, va));
                p3a.Points.AddXY(ta, 0);
                p3b.Points.AddXY(ta, va);
                aDone = true;
            }
            if (hb > 0)
            {
                z1b.Points.AddXY(tb, hb);
                z2b.Points.AddXY(tb, vb);
            }
            else if (!bDone)
            {
                richTextBox1.AppendText(string.Format("(step/2) Nusileidimas ant žemės, aukštis: {0, 0:F3}m, laikas nuo iššokimo: {1, 0:F3}s, greitis: {2, 0:F3}m/s\n", hb, tb, vb));
                p3a.Points.AddXY(tb, 0);
                p3b.Points.AddXY(tb, vb);
                bDone = true;
            }
            if (aDone && bDone)
            {
                richTextBox1.AppendText(line + "\n");
                richTextBox1.AppendText("Baigta skaičiuoti.\n");
                timer1.Stop();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            ClearForm1();
        }
        public void ClearForm1()
        {
            richTextBox1.Clear();
            //0,34 - runges ir kutos
            //0,21 - eulerio
            pradinesReiksmes(0.21);
            chart1.Series.Clear();
            chart2.Series.Clear();
        }
    }
}