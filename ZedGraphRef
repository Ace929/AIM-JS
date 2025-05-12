// Program.cs  —  ZedGraph “kitchen-sink” demo
// ------------------------------------------------------------
// Builds one GraphPane per demo and exports to /out/*.png

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZedGraph;

internal static class Program
{
    private const int W = 1280, H = 720;                 // export size

    private static readonly string OUT = Path.Combine(
        AppContext.BaseDirectory, "out");

    private static void Main()
    {
        Directory.CreateDirectory(OUT);

        MakeLinePlot();
        MakeScatterWithFit();
        MakeAreaFill();
        MakeDualAxisPlot();
        MakeBarPlusLine();
        MakeHistogram();
        MakePie();
        MakeAnnotations();
        MakeLogPlot();
        MakeMultiPaneDash();

        Console.WriteLine("\n✔ All demos exported to:");
        Console.WriteLine(OUT);
        OpenExplorer(OUT);
    }

    // --------------------------------------------------------
    //  Helper: render GraphPane -> PNG
    // --------------------------------------------------------
    private static void Save(GraphPane pane, string fileName)
    {
        using var bmp = new Bitmap(W, H);
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            pane.Draw(g, new RectangleF(0, 0, W, H));
        }

        var path = Path.Combine(OUT, fileName);
        bmp.Save(path, ImageFormat.Png);
        Console.WriteLine($"   · {fileName}");
    }

    // --------------------------------------------------------
    // 1) Basic line + symbol curve with gradient stroke
    // --------------------------------------------------------
    private static void MakeLinePlot()
    {
        var pane = new GraphPane
        {
            Title = { Text = "Sine vs. Cosine" },
            Chart = { Fill = new Fill(Color.White) },
            Fill  = new Fill(Color.FromArgb(245, 245, 245))
        };
        StyleAxis(pane.XAxis, "θ (radians)");
        StyleAxis(pane.YAxis, "Value");

        var pts1 = new PointPairList();
        var pts2 = new PointPairList();
        for (double x = 0; x <= 2 * Math.PI; x += 0.1)
        {
            pts1.Add(x, Math.Sin(x));
            pts2.Add(x, Math.Cos(x));
        }

        var c1 = pane.AddCurve(
            "sin θ", pts1, Color.RoyalBlue, SymbolType.Circle);
        c1.Symbol.Size = 6;
        c1.Line.Width = 2.5f;
        c1.Line.Fill = new Fill(Color.RoyalBlue, Color.MidnightBlue, 90);

        var c2 = pane.AddCurve(
            "cos θ", pts2, Color.OrangeRed, SymbolType.Diamond);
        c2.Symbol.Fill = new Fill(Color.White);
        c2.Symbol.Size = 6;
        c2.Line.Width = 2.5f;

        pane.Legend.Position = LegendPos.TopCenter;

        Save(pane, "01_line.png");
    }

    // --------------------------------------------------------
    // 2) Scatter plot + least-squares regression line
    // --------------------------------------------------------
    private static void MakeScatterWithFit()
    {
        var pane = new GraphPane
        {
            Title = { Text = "Random scatter with fit" },
            Chart = { Fill = new Fill(Color.White) },
        };
        StyleAxis(pane.XAxis, "X");
        StyleAxis(pane.YAxis, "Y");

        var rng  = new Random(7);
        var pts  = new PointPairList();
        for (int i = 0; i < 50; i++)
        {
            double x = i / 5.0;
            double y = 2.0 * x + 3 + rng.NextDouble() * 5.0 - 2.5; // noisy
            pts.Add(x, y);
        }

        var scatter = pane.AddCurve("samples", pts,
            Color.MediumSeaGreen, SymbolType.Square);
        scatter.Line.IsVisible = false;
        scatter.Symbol.Size = 7;
        scatter.Symbol.Fill = new Fill(Color.White);

        // least-squares fit
        (double m, double b) = FitLine(pts);
        var fitPts = new PointPairList
        {
            new PointPair(pts[0].X, m * pts[0].X + b),
            new PointPair(pts[^1].X, m * pts[^1].X + b)
        };
        var fit = pane.AddCurve($"y = {m:F2}x + {b:F2}",
            fitPts, Color.DarkSlateBlue, SymbolType.None);
        fit.Line.Width = 3;

        Save(pane, "02_scatter_fit.png");
    }

    // LSQ helper
    private static (double m, double b) FitLine(PointPairList p)
    {
        double sx = 0, sy = 0, sxy = 0, sx2 = 0;
        int n = p.Count;
        foreach (var pt in p)
        {
            sx  += pt.X; sy += pt.Y;
            sxy += pt.X * pt.Y;
            sx2 += pt.X * pt.X;
        }
        double m = (n * sxy - sx * sy) / (n * sx2 - sx * sx);
        double b = (sy - m * sx) / n;
        return (m, b);
    }

    // --------------------------------------------------------
    // 3) Area fill under curve
    // --------------------------------------------------------
    private static void MakeAreaFill()
    {
        var pane = new GraphPane
        {
            Title = { Text = "Filled area plot" },
            Chart = { Fill = new Fill(Color.White) },
        };
        StyleAxis(pane.XAxis, "Time");
        StyleAxis(pane.YAxis, "Value");

        var pts = new PointPairList();
        var rng = new Random(3);
        double y = 0;
        for (int i = 0; i < 100; i++)
        {
            y += rng.NextDouble() * 10 - 5;
            pts.Add(i, y);
        }

        var curve = pane.AddCurve("cumulative",
            pts, Color.SteelBlue, SymbolType.None);
        curve.Line.Width = 2;
        curve.Line.Fill = new Fill(
            Color.FromArgb(120, Color.SteelBlue), Color.Transparent);

        Save(pane, "03_area_fill.png");
    }

    // --------------------------------------------------------
    // 4) Primary & secondary Y axes in same pane
    // --------------------------------------------------------
    private static void MakeDualAxisPlot()
    {
        var pane = new GraphPane
        {
            Title = { Text = "Dual Y-axes" },
            Chart = { Fill = new Fill(Color.White) },
        };
        StyleAxis(pane.XAxis, "Day");
        StyleAxis(pane.YAxis, "Temp °C");
        StyleAxis(pane.Y2Axis, "Revenue $k");

        pane.Y2Axis.IsVisible = true;            // enable secondary

        var rng = new Random(1);
        var tPts = new PointPairList();
        var rPts = new PointPairList();
        for (int d = 1; d <= 30; d++)
        {
            tPts.Add(d, 20 + rng.NextDouble() * 10);
            rPts.Add(d, 50 + rng.NextDouble() * 40);
        }

        var temp = pane.AddCurve("Temp", tPts, Color.Crimson, SymbolType.Circle);
        temp.Symbol.Size = 5;
        temp.Line.Width  = 2.5f;

        var rev  = pane.AddCurve("Revenue", rPts, Color.SeaGreen, SymbolType.Diamond);
        rev.IsY2Axis = true;                     // <-- IMPORTANT
        rev.Symbol.Size = 5;
        rev.Line.Width  = 2.5f;

        pane.Legend.Position = LegendPos.BottomFlushCenter;
        Save(pane, "04_dual_axis.png");
    }

    // --------------------------------------------------------
    // 5) Column chart + overlay line
    // --------------------------------------------------------
    private static void MakeBarPlusLine()
    {
        var pane = new GraphPane
        {
            Title = { Text = "Sales vs. target" },
            Chart = { Fill = new Fill(Color.White) },
        };
        StyleAxis(pane.XAxis, "Quarter");
        StyleAxis(pane.YAxis, "Units");

        string[] q = { "Q1", "Q2", "Q3", "Q4" };
        double[] sales = { 120, 150, 130, 170 };
        double[] target = { 140, 140, 140, 140 };

        var bar = pane.AddBar("Sales", null, sales, Color.DeepSkyBlue);
        bar.Bar.Fill = new Fill(Color.DeepSkyBlue, Color.DodgerBlue, 90);
        bar.Bar.Border.IsVisible = false;
        bar.Bar.Width = 0.5;

        var tPts = new PointPairList();
        for (int i = 0; i < q.Length; i++)
            tPts.Add(i + 1, target[i]);

        var tgt = pane.AddCurve("Target", tPts, Color.OrangeRed, SymbolType.Triangle);
        tgt.Line.Width = 3;

        pane.XAxis.Type = AxisType.Text;
        pane.XAxis.TextLabels = q;

        Save(pane, "05_bar_plus_line.png");
    }

    // --------------------------------------------------------
    // 6) Histogram (bins auto-computed)
    // --------------------------------------------------------
    private static void MakeHistogram()
    {
        var rng   = new Random(42);
        var data  = new double[1000];
        for (int i = 0; i < data.Length; i++)
            data[i] = rng.NextGaussian(mu: 50, sigma: 15); // ext. method below

        var pane = new GraphPane
        {
            Title = { Text = "Histogram (μ=50, σ=15)" },
            Chart = { Fill = new Fill(Color.White) },
        };

        StyleAxis(pane.XAxis, "Value");
        StyleAxis(pane.YAxis, "Count");

        int binCount = 25;
        double min = double.MaxValue, max = double.MinValue;
        foreach (var v in data) { if (v < min) min = v; if (v > max) max = v; }
        double w = (max - min) / binCount;

        var bins = new int[binCount];
        foreach (var v in data)
            bins[Math.Min((int)((v - min) / w), binCount - 1)]++;

        var xs = new double[binCount];
        var ys = new double[binCount];
        for (int i = 0; i < binCount; i++)
        {
            xs[i] = min + i * w + w / 2.0;
            ys[i] = bins[i];
        }

        var hist = pane.AddBar("freq", xs, ys, Color.MediumPurple);
        hist.Bar.Fill = new Fill(Color.MediumPurple, Color.Indigo, 90);
        hist.Bar.Border.IsVisible = false;
        hist.Bar.Width = w * 0.9;

        pane.BarSettings.Type = BarType.Cluster;
        pane.Legend.IsVisible = false;

        Save(pane, "06_histogram.png");
    }

    // --------------------------------------------------------
    // 7) Pie chart
    // --------------------------------------------------------
    private static void MakePie()
    {
        var pane = new GraphPane
        {
            Title = { Text = "Portfolio Allocation" },
            Legend = { Position = LegendPos.Right },
        };
        pane.Fill = new Fill(Color.White);

        string[] lbls = { "Equities", "Bonds", "Real Estate", "Cash" };
        double[] vals = { 55, 25, 15, 5 };
        Color[]  cols = { Color.SteelBlue, Color.Goldenrod,
                          Color.MediumSeaGreen, Color.Silver };

        for (int i = 0; i < lbls.Length; i++)
        {
            var slice = pane.AddPieSlice(vals[i], cols[i], 0, lbls[i]);
            slice.LabelType = PieLabelType.Name_Percent;  // show both
            slice.LabelDetail.FontSpec.Size = 12f;
        }

        Save(pane, "07_pie.png");
    }

    // --------------------------------------------------------
    // 8) Annotations (text, arrows, boxes)
    // --------------------------------------------------------
    private static void MakeAnnotations()
    {
        var pane = new GraphPane
        {
            Title = { Text = "Annotations demo" },
            Chart = { Fill = new Fill(Color.White) },
        };
        StyleAxis(pane.XAxis, "X");
        StyleAxis(pane.YAxis, "Y");

        // background & drop shadow
        pane.Fill = new Fill(Color.WhiteSmoke, Color.LightGray, 45);

        // a simple curve
        var pts = new PointPairList();
        for (double x = -5; x <= 5; x += 0.2)
            pts.Add(x, Math.Sin(x) / x);
        var c = pane.AddCurve("sinc", pts, Color.Navy, SymbolType.None);
        c.Line.Width = 3;

        // text annotation
        var txt = new TextObj("Peak", 1.5, 0.66,
            CoordType.AxisXYScale, AlignH.Center, AlignV.Bottom);
        txt.FontSpec.Size = 14;
        txt.FontSpec.Border.IsVisible = false;
        txt.FontSpec.Fill = new Fill(Color.FromArgb(180, Color.Yellow));
        pane.GraphObjList.Add(txt);

        // arrow
        var arr = new ArrowObj(Color.Red, 15f, 1.5, 0.6, 1.5, 0.0);
        arr.Location.CoordinateFrame = CoordType.AxisXYScale;
        pane.GraphObjList.Add(arr);

        // a translucent highlight box
        var box = new BoxObj(-1, 0.3, 2, 0.6,
            Color.Empty, Color.FromArgb(60, Color.Crimson));
        box.Location.CoordinateFrame = CoordType.AxisXYScale;
        box.Border.IsVisible = false;
        pane.GraphObjList.Add(box);

        Save(pane, "08_annotations.png");
    }

    // --------------------------------------------------------
    // 9) Log scale with date-time X axis
    // --------------------------------------------------------
    private static void MakeLogPlot()
    {
        var pane = new GraphPane
        {
            Title = { Text = "Semi-log plot of manufactured parts" },
            Chart = { Fill = new Fill(Color.White) },
        };
        pane.XAxis.Type = AxisType.Date;
        pane.XAxis.Title.Text = "Date";
        pane.YAxis.Title.Text = "Defects (log)";
        pane.YAxis.Type = AxisType.Log;
        pane.YAxis.Scale.Min = 1;

        var pts = new PointPairList();
        var start = new DateTime(2025, 1, 1);
        var rng = new Random(11);
        double d = 500;
        for (int i = 0; i < 60; i++)
        {
            d = d * (0.97 + rng.NextDouble() * 0.06);  // trending down
            pts.Add(new XDate(start.AddDays(i)), d);
        }

        var curve = pane.AddCurve("Defects",
            pts, Color.Firebrick, SymbolType.None);
        curve.Line.Width = 3;
        curve.Line.Fill = new Fill(Color.Firebrick, Color.OrangeRed, 90);

        Save(pane, "09_log_plot.png");
    }

    // --------------------------------------------------------
    // 10) Dashboard: multiple panes on one bitmap
    // --------------------------------------------------------
    private static void MakeMultiPaneDash()
    {
        // Build 3 small panes ➜ blend into one master
        var master = new MasterPane("Dashboard", new Rectangle(0, 0, W, H))
        {
            Legend = { IsVisible = false },
            IsFontsScaled = false
        };
        master.Margin.All = 10;
        master.Fill = new Fill(Color.WhiteSmoke, Color.Gainsboro, 90);

        GraphPane[] panes =
        {
            ClonePaneForMini("Mini-hist"), ClonePaneForMini("Mini-line"),
            ClonePaneForMini("Mini-pie")
        };

        // 1) tiny histogram
        {
            var rng = new Random(5);
            var xs = new double[100];
            for (int i = 0; i < xs.Length; i++) xs[i] = rng.NextGaussian(0,1);
            BuildMiniHist(panes[0], xs);
        }

        // 2) spark-like line
        {
            var p = panes[1];
            p.Chart.Fill = new Fill(Color.White);
            var pl = new PointPairList();
            for (int i = 0; i < 50; i++)
                pl.Add(i, Math.Sin(i * 0.2) + i * 0.05);
            var c = p.AddCurve("", pl, Color.DodgerBlue, SymbolType.None);
            c.Line.Width = 2;
            p.XAxis.IsVisible = p.YAxis.IsVisible = false;
        }

        // 3) tiny pie
        {
            var p = panes[2];
            p.Chart.Fill = new Fill(Color.White);
            var s1 = p.AddPieSlice(40, Color.SteelBlue, 0, "");
            var s2 = p.AddPieSlice(60, Color.SlateGray, 0, "");
            s1.LabelType = s2.LabelType = PieLabelType.None;
        }

        master.Add(panes);
        master.SetLayout(new Graphics(null), PaneLayout.SquareColPreferred);

        // render
        using var bmp = new Bitmap(W, H);
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            master.Draw(g);
        }
        bmp.Save(Path.Combine(OUT, "10_dashboard.png"), ImageFormat.Png);
        Console.WriteLine("   · 10_dashboard.png");
    }

    // ---- helpers for dashboard ----
    private static GraphPane ClonePaneForMini(string title) =>
        new()
        {
            Title = { Text = title, FontSpec = { Size = 12 } },
            Margin = { All = 10 }
        };
    private static void BuildMiniHist(GraphPane p, double[] data)
    {
        int bins = 15;
        (double mn, double mx) = (double.MaxValue, double.MinValue);
        foreach (var v in data) { if (v < mn) mn = v; if (v > mx) mx = v; }
        double w = (mx - mn) / bins;
        var cnt = new int[bins];
        foreach (var v in data) cnt[Math.Min((int)((v - mn) / w), bins - 1)]++;

        var xs = new double[bins];
        var ys = new double[bins];
        for (int i = 0; i < bins; i++) { xs[i] = mn + i * w + w / 2; ys[i] = cnt[i]; }

        var bar = p.AddBar("", xs, ys, Color.SlateBlue);
        bar.Bar.Fill = new Fill(Color.SlateBlue);
        bar.Bar.Border.IsVisible = false;
        bar.Bar.Width = w * .9;
        p.XAxis.IsVisible = p.YAxis.IsVisible = false;
    }

    // --------------------------------------------------------
    //  Fancy axis styling utility  (reuse everywhere)
    // --------------------------------------------------------
    private static void StyleAxis(Axis ax, string title)
    {
        ax.Title.Text        = title;
        ax.Title.FontSpec.Size = 14f;
        ax.Scale.FontSpec.Size = 12f;
        ax.MajorGrid.IsVisible = true;
        ax.MajorGrid.Color     = Color.FromArgb(50, 50, 50, 50);
    }

    // --------------------------------------------------------
    //  Try to open platform file explorer on completion
    // --------------------------------------------------------
    private static void OpenExplorer(string folder)
    {
        try
        {
            var psi = OperatingSystem.IsWindows()
                ? new ProcessStartInfo("explorer", $"\"{folder}\"")
                : OperatingSystem.IsMacOS()
                    ? new ProcessStartInfo("open", folder)
                    : new ProcessStartInfo("xdg-open", folder);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }
        catch { /* ignore */ }
    }

    // --------------------------------------------------------
    //  Extension method: Gaussian RNG (Box-Muller)
    // --------------------------------------------------------
    private static double NextGaussian(this Random rng,
        double mu = 0.0, double sigma = 1.0)
    {
        // two-call cache
        if (_hasSpare)
        {
            _hasSpare = false;
            return _spare * sigma + mu;
        }
        double u, v, s;
        do
        {
            u = rng.NextDouble() * 2 - 1;
            v = rng.NextDouble() * 2 - 1;
            s = u * u + v * v;
        } while (s >= 1 || s == 0);
        s = Math.Sqrt(-2.0 * Math.Log(s) / s);
        _spare = v * s;
        _hasSpare = true;
        return mu + sigma * u * s;
    }
    private static bool _hasSpare; private static double _spare;
}
