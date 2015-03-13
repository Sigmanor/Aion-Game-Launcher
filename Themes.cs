using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public class Draw
    {
        public static void Gradient(Graphics g, Color c1, Color c2, int x, int y, int width, int height)
        {
            Rectangle R = new Rectangle(x, y, width, height);
            using (LinearGradientBrush T = new LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical))
            {
                g.FillRectangle(T, R);
            }
        }
        public static void Blend(Graphics g, Color c1, Color c2, Color c3, float c, int d, int x, int y, int width, int height)
        {
            ColorBlend V = new ColorBlend(3);
            V.Colors = new Color[] { c1, c2, c3 };
            V.Positions = new float[] { 0F, c, 1F };
            Rectangle R = new Rectangle(x, y, width, height);
            using (LinearGradientBrush T = new LinearGradientBrush(R, c1, c1, (LinearGradientMode)d))
            {
                T.InterpolationColors = V;
                g.FillRectangle(T, R);
            }
        }
    }

    class Pigment
    {
        public string Name { get; set; }
        public Color Value { get; set; }

        public Pigment()
        {
        }

        public Pigment(string n, Color v)
        {
            Name = n;
            Value = v;
        }

        public Pigment(string n, byte a, byte r, byte g, byte b)
        {
            Name = n;
            Value = Color.FromArgb(a, r, g, b);
        }

        public Pigment(string n, byte r, byte g, byte b)
        {
            Name = n;
            Value = Color.FromArgb(r, g, b);
        }
    }

    class FTheme : ContainerControl
    {

        public bool Resizeable { get; set; }

        public FTheme()
        {
            SetStyle((ControlStyles)8198, true);
            Pigment[] C = new Pigment[]{
            new Pigment("Border", Color.Black),
            new Pigment("Frame", 47, 47, 50),
            new Pigment("Border Highlight", 15, 255, 255, 255),
            new Pigment("Side Highlight", 6, 255, 255, 255),
            new Pigment("Shine", 20, 255, 255, 255),
            new Pigment("Shadow", 38, 38, 40),
            new Pigment("Backcolor", 247, 247, 251),
            new Pigment("Transparency", Color.Fuchsia)
        };
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            Dock = DockStyle.Fill;
            if (Parent is Form)
                ((Form)Parent).FormBorderStyle = FormBorderStyle.None;
            Colors = C;
            base.OnHandleCreated(e);
        }

        const byte Count = 8;
        private Pigment[] C = new Pigment[]{
            new Pigment("Border", Color.Black),
            new Pigment("Frame", 47, 47, 50),
            new Pigment("Border Highlight", 15, 255, 255, 255),
            new Pigment("Side Highlight", 6, 255, 255, 255),
            new Pigment("Shine", 20, 255, 255, 255),
            new Pigment("Shadow", 38, 38, 40),
            new Pigment("Backcolor", 247, 247, 251),
            new Pigment("Transparency", Color.Fuchsia)
        };
        public Pigment[] Colors
        {
            get { return C; }
            set
            {
                if (value.Length != Count)
                    throw new IndexOutOfRangeException();

                P1 = new Pen(value[0].Value);
                P2 = new Pen(value[2].Value);

                B1 = new SolidBrush(value[6].Value);
                B2 = new SolidBrush(value[7].Value);

                if (Parent != null)
                {
                    Parent.BackColor = value[6].Value;
                    if (Parent is Form)
                        ((Form)Parent).TransparencyKey = value[7].Value;
                }

                CB = new ColorBlend();
                CB.Colors = new Color[]{
                Color.Transparent,
                value[4].Value,
                Color.Transparent
            };
                CB.Positions = new float[] {
                0,
                (float)0.5,
                1
            };

                C = value;

                Invalidate();
            }
        }

        private Pen P1;
        private Pen P2;
        //private Pen P3;
        private SolidBrush B1;
        private SolidBrush B2;
        private LinearGradientBrush B3;
        private LinearGradientBrush B4;
        private Rectangle R1;
        private Rectangle R2;

        private ColorBlend CB;
        private Graphics G;
        private Bitmap B;
        protected override void OnPaint(PaintEventArgs e)
        {
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);

            G.Clear(C[1].Value);

            G.DrawRectangle(P2, new Rectangle(1, 1, Width - 3, Height - 3));
            G.DrawRectangle(P2, new Rectangle(12, 40, Width - 24, Height - 52));

            R1 = new Rectangle(1, 0, 15, Height);
            B3 = new LinearGradientBrush(R1, C[3].Value, Color.Transparent, 90f);
            G.FillRectangle(B3, R1);
            G.FillRectangle(B3, new Rectangle(Width - 16, 0, 15, Height));

            G.FillRectangle(B1, new Rectangle(13, 41, Width - 26, Height - 54));

            R2 = new Rectangle(0, 2, Width, 2);
            B4 = new LinearGradientBrush(R2, Color.Empty, Color.Empty, 0F);
            B4.InterpolationColors = CB;
            G.FillRectangle(B4, R2);

            G.DrawRectangle(P1, new Rectangle(13, 41, Width - 26, Height - 54));
            G.DrawRectangle(P1, new Rectangle(0, 0, Width - 1, Height - 1));

            G.FillRectangle(B2, new Rectangle(0, 0, 2, 2));
            G.FillRectangle(B2, new Rectangle(Width - 2, 0, 2, 2));
            G.FillRectangle(B2, new Rectangle(Width - 2, Height - 2, 2, 2));
            G.FillRectangle(B2, new Rectangle(0, Height - 2, 2, 2));

            B.SetPixel(1, 1, Color.Black);
            B.SetPixel(Width - 2, 1, Color.Black);
            B.SetPixel(Width - 2, Height - 2, Color.Black);
            B.SetPixel(1, Height - 2, Color.Black);

            e.Graphics.DrawImage(B, 0, 0);
            B3.Dispose();
            B4.Dispose();
            G.Dispose();
            B.Dispose();
        }

        public enum Direction : int
        {
            NONE = 0,
            LEFT = 10,
            RIGHT = 11,
            TOP = 12,
            TOPLEFT = 13,
            TOPRIGHT = 14,
            BOTTOM = 15,
            BOTTOMLEFT = 16,
            BOTTOMRIGHT = 17
        }
        private Direction Current;
        public void SetCurrent()
        {
            Point T = PointToClient(MousePosition);
            if (T.X < 7 & T.Y < 7)
            {
                Current = Direction.TOPLEFT;
                Cursor = Cursors.SizeNWSE;
            }
            else if (T.X < 7 & T.Y > Height - 7)
            {
                Current = Direction.BOTTOMLEFT;
                Cursor = Cursors.SizeNESW;
            }
            else if (T.X > Width - 7 & T.Y > Height - 7)
            {
                Current = Direction.BOTTOMRIGHT;
                Cursor = Cursors.SizeNWSE;
            }
            else if (T.X > Width - 7 & T.Y < 7)
            {
                Current = Direction.TOPRIGHT;
                Cursor = Cursors.SizeNESW;
            }
            else if (T.X < 7)
            {
                Current = Direction.LEFT;
                Cursor = Cursors.SizeWE;
            }
            else if (T.X > Width - 7)
            {
                Current = Direction.RIGHT;
                Cursor = Cursors.SizeWE;
            }
            else if (T.Y < 7)
            {
                Current = Direction.TOP;
                Cursor = Cursors.SizeNS;
            }
            else if (T.Y > Height - 7)
            {
                Current = Direction.BOTTOM;
                Cursor = Cursors.SizeNS;
            }
            else
            {
                Current = Direction.NONE;
                Cursor = Cursors.Default;
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Parent is Form)
                {
                    if (((Form)Parent).WindowState == FormWindowState.Maximized)
                        return;
                }
                if (Drag.Contains(e.Location))
                {
                    Capture = false;
                    IntPtr Val = new IntPtr(2);
                    IntPtr NULL = IntPtr.Zero;
                    Message msg = Message.Create(Parent.Handle, 161, Val, NULL);
                    DefWndProc(ref msg);
                }
                else
                {
                    if (Current != Direction.NONE & Resizeable)
                    {
                        Capture = false;
                        IntPtr Val = new IntPtr(Convert.ToInt32(Current));
                        IntPtr NULL = IntPtr.Zero;
                        Message msg = Message.Create(Parent.Handle, 161, Val, NULL);
                        DefWndProc(ref msg);
                    }
                }
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Resizeable)
                SetCurrent();
            base.OnMouseMove(e);
        }
        protected override void OnSizeChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnSizeChanged(e);
        }
        private Rectangle Drag
        {
            get { return new Rectangle(7, 7, Width - 14, 35); }
        }

    }
    class FButton : Control
    {

        private bool Shadow_ = true;
        public bool Shadow
        {
            get { return Shadow_; }
            set
            {
                Shadow_ = value;
                Invalidate();
            }
        }

        public FButton()
        {
            SetStyle((ControlStyles)8198, true);
            Colors = new Pigment[] {
            new Pigment("Border", 254, 133, 0),
            new Pigment("Backcolor", 247, 247, 251),
            new Pigment("Highlight", 255, 197, 19),
            new Pigment("Gradient1", 255, 175, 12),
            new Pigment("Gradient2", 255, 127, 1),
            new Pigment("Text Color", Color.White),
            new Pigment("Text Shadow", 30, 0, 0, 0)
        };
            Font = new Font("Verdana", 8);
        }

        const byte Count = 7;
        private Pigment[] C;
        public Pigment[] Colors
        {
            get { return C; }
            set
            {
                if (value.Length != Count)
                    throw new IndexOutOfRangeException();

                P1 = new Pen(value[0].Value);
                P2 = new Pen(value[2].Value);

                B1 = new SolidBrush(value[6].Value);
                B2 = new SolidBrush(value[5].Value);

                C = value;
                Invalidate();
            }
        }

        private Pen P1;
        private Pen P2;
        private SolidBrush B1;
        private SolidBrush B2;
        private LinearGradientBrush B3;
        private Size SZ;

        private Point PT;
        private Graphics G;
        private Bitmap B;
        protected override void OnPaint(PaintEventArgs e)
        {
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);

            if (Down)
            {
                B3 = new LinearGradientBrush(ClientRectangle, C[4].Value, C[3].Value, 90f);
            }
            else
            {
                B3 = new LinearGradientBrush(ClientRectangle, C[3].Value, C[4].Value, 90f);
            }
            G.FillRectangle(B3, ClientRectangle);

            if (!string.IsNullOrEmpty(Text))
            {
                SZ = G.MeasureString(Text, Font).ToSize();
                PT = new Point(Convert.ToInt32(Width / 2 - SZ.Width / 2), Convert.ToInt32(Height / 2 - SZ.Height / 2));
                if (Shadow_)
                    G.DrawString(Text, Font, B1, PT.X + 1, PT.Y + 1);
                G.DrawString(Text, Font, B2, PT);
            }

            G.DrawRectangle(P1, new Rectangle(0, 0, Width - 1, Height - 1));
            G.DrawRectangle(P2, new Rectangle(1, 1, Width - 3, Height - 3));

            B.SetPixel(0, 0, C[1].Value);
            B.SetPixel(Width - 1, 0, C[1].Value);
            B.SetPixel(Width - 1, Height - 1, C[1].Value);
            B.SetPixel(0, Height - 1, C[1].Value);

            e.Graphics.DrawImage(B, 0, 0);
            B3.Dispose();
            G.Dispose();
            B.Dispose();
        }

        private bool Down;
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Down = true;
                Invalidate();
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            Down = false;
            Invalidate();
            base.OnMouseUp(e);
        }

    }
    class FProgressBar : Control
    {

        private double _Maximum = 100;
        public double Maximum
        {
            get { return _Maximum; }
            set
            {
                _Maximum = value;
                Progress = _Current / value * 100;
            }
        }
        private double _Current;
        public double Current
        {
            get { return _Current; }
            set { Progress = value / _Maximum * 100; }
        }
        private double _Progress;
        public double Progress
        {
            get { return _Progress; }
            set
            {
                if (value < 0)
                    value = 0;
                else
                    if (value > 100)
                        value = 100;
                _Progress = value;
                _Current = value * 0.01 * _Maximum;
                Invalidate();
            }
        }

        public FProgressBar()
        {
            SetStyle((ControlStyles)8198, true);
            Colors = new Pigment[] {
            new Pigment("Border", 214, 214, 216),
            new Pigment("Backcolor1", 247, 247, 251),
            new Pigment("Backcolor2", 239, 239, 242),
            new Pigment("Highlight", 100, 255, 255, 255),
            new Pigment("Forecolor", 224, 224, 224),
            new Pigment("Gloss", 130, 255, 255, 255)
        };
        }

        const byte Count = 6;
        private Pigment[] C;
        public Pigment[] Colors
        {
            get { return C; }
            set
            {
                if (value.Length != Count)
                    throw new IndexOutOfRangeException();

                P1 = new Pen(value[0].Value);
                P2 = new Pen(value[3].Value);

                B1 = new SolidBrush(value[4].Value);

                CB = new ColorBlend();
                CB.Colors = new Color[] {
                value[5].Value,
                Color.Transparent,
                Color.Transparent
            };
                CB.Positions = new float[]{
                0,
                0.3F,
                1
            };

                C = value;
                Invalidate();
            }
        }

        private Pen P1;
        private Pen P2;
        private SolidBrush B1;
        private LinearGradientBrush B2;

        private ColorBlend CB;
        private Graphics G;
        private Bitmap B;
        protected override void OnPaint(PaintEventArgs e)
        {
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);

            G.Clear(C[2].Value);

            G.FillRectangle(B1, new Rectangle(1, 1, Convert.ToInt32((Width * _Progress * 0.01) - 2), Height - 2));

            B2 = new LinearGradientBrush(ClientRectangle, Color.Empty, Color.Empty, 90f);
            B2.InterpolationColors = CB;
            G.FillRectangle(B2, ClientRectangle);

            G.DrawRectangle(P1, new Rectangle(0, 0, Width - 1, Height - 1));
            G.DrawRectangle(P2, new Rectangle(1, 1, Width - 3, Height - 3));

            B.SetPixel(0, 0, C[1].Value);
            B.SetPixel(Width - 1, 0, C[1].Value);
            B.SetPixel(Width - 1, Height - 1, C[1].Value);
            B.SetPixel(0, Height - 1, C[1].Value);

            e.Graphics.DrawImage(B, 0, 0);
            B2.Dispose();
            G.Dispose();
            B.Dispose();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            Invalidate();
            base.OnSizeChanged(e);
        }
    }

    abstract class Theme : ContainerControl
    {

        #region " Initialization "

        protected Bitmap B;
        protected Graphics G;
        public Theme()
        {
            SetStyle((ControlStyles)8198, true);
            B = new Bitmap(1, 1);
            G = Graphics.FromImage(B);
        }

        private bool ParentIsForm;
        protected override void OnHandleCreated(EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentIsForm = Parent is Form;
            if (ParentIsForm)
                ParentForm.FormBorderStyle = FormBorderStyle.None;
            base.OnHandleCreated(e);
        }

        #endregion

        #region " Sizing and Movement "

        private bool _Resizable = true;
        public bool Resizable
        {
            get { return _Resizable; }
            set { _Resizable = value; }
        }

        private int _MoveHeight = 24;
        public int MoveHeight
        {
            get { return _MoveHeight; }
            set
            {
                _MoveHeight = value;
                Header = new Rectangle(7, 7, Width - 14, _MoveHeight);
            }
        }

        private IntPtr Flag;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!(e.Button == MouseButtons.Left))
                return;
            if (ParentIsForm)
                if (ParentForm.WindowState == FormWindowState.Maximized)
                    return;

            if (Header.Contains(e.Location))
            {
                Flag = new IntPtr(2);
            }
            else if (Current.Position == 0 | !_Resizable)
            {
                return;
            }
            else
            {
                Flag = new IntPtr(Current.Position);
            }

            Capture = false;
            Message msg = Message.Create(Parent.Handle, 161, Flag, IntPtr.Zero);
            DefWndProc(ref msg);

            base.OnMouseDown(e);
        }

        private struct Pointer
        {
            public readonly Cursor Cursor;
            public readonly byte Position;
            public Pointer(Cursor c, byte p)
            {
                Cursor = c;
                Position = p;
            }
        }

        private bool F1;
        private bool F2;
        private bool F3;
        private bool F4;
        private Point PTC;
        private Pointer GetPointer()
        {
            PTC = PointToClient(MousePosition);
            F1 = PTC.X < 7;
            F2 = PTC.X > Width - 7;
            F3 = PTC.Y < 7;
            F4 = PTC.Y > Height - 7;

            if (F1 & F3)
                return new Pointer(Cursors.SizeNWSE, 13);
            if (F1 & F4)
                return new Pointer(Cursors.SizeNESW, 16);
            if (F2 & F3)
                return new Pointer(Cursors.SizeNESW, 14);
            if (F2 & F4)
                return new Pointer(Cursors.SizeNWSE, 17);
            if (F1)
                return new Pointer(Cursors.SizeWE, 10);
            if (F2)
                return new Pointer(Cursors.SizeWE, 11);
            if (F3)
                return new Pointer(Cursors.SizeNS, 12);
            if (F4)
                return new Pointer(Cursors.SizeNS, 15);
            return new Pointer(Cursors.Default, 0);
        }

        private Pointer Current;
        private Pointer Pending;
        private void SetCurrent()
        {
            Pending = GetPointer();
            if (Current.Position == Pending.Position)
                return;
            Current = GetPointer();
            Cursor = Current.Cursor;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_Resizable)
                SetCurrent();
            base.OnMouseMove(e);
        }

        protected Rectangle Header;
        protected override void OnSizeChanged(EventArgs e)
        {
            Header = new Rectangle(7, 7, Width - 14, _MoveHeight);
            G.Dispose();
            B.Dispose();
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);
            Invalidate();
            base.OnSizeChanged(e);
        }

        #endregion

        #region " Convienence "

        public void SetTransparent(Color c)
        {
            if (ParentIsForm)
                ParentForm.TransparencyKey = c;
        }

        protected override abstract void OnPaint(PaintEventArgs e);

        public void DrawCorners(Color c, Rectangle rect)
        {
            B.SetPixel(rect.X, rect.Y, c);
            B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c);
            B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c);
            B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c);
        }

        public void DrawBorders(Pen p1, Pen p2, Rectangle rect)
        {
            G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
        }

        private Size TextSize;
        public void DrawText(HorizontalAlignment a, Brush b, int offset = 0)
        {
            if (string.IsNullOrEmpty(Text))
                return;
            TextSize = G.MeasureString(Text, Font).ToSize();

            switch (a)
            {
                case HorizontalAlignment.Left:
                    G.DrawString(Text, Font, b, 5 + offset, _MoveHeight / 2 - TextSize.Height / 2 + 7);
                    break;
                case HorizontalAlignment.Right:
                    G.DrawString(Text, Font, b, Width - 5 - TextSize.Width - offset, _MoveHeight / 2 - TextSize.Height / 2 + 7);
                    break;
                case HorizontalAlignment.Center:
                    G.DrawString(Text, Font, b, Width / 2 - TextSize.Width / 2, _MoveHeight / 2 - TextSize.Height / 2 + 7);
                    break;
            }
        }

        public int ImageWidth
        {
            get
            {
                if (_Image == null)
                    return 0;
                return _Image.Width;
            }
        }

        private Image _Image;
        public Image Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                Invalidate();
            }
        }

        public void DrawIcon(HorizontalAlignment a, int offset = 0)
        {
            if (_Image == null)
                return;
            switch (a)
            {
                case HorizontalAlignment.Left:
                    G.DrawImage(_Image, 5 + offset, _MoveHeight / 2 - _Image.Height / 2 + 7);
                    break;
                case HorizontalAlignment.Right:
                    G.DrawImage(_Image, Width - 5 - TextSize.Width - offset, _MoveHeight / 2 - TextSize.Height / 2 + 7);
                    break;
                case HorizontalAlignment.Center:
                    G.DrawImage(_Image, Width / 2 - TextSize.Width / 2, _MoveHeight / 2 - TextSize.Height / 2 + 7);
                    break;
            }
        }
        #endregion

    }
    abstract class ThemeControl : Control
    {

        #region " Initialization "

        protected Bitmap B;
        protected Graphics G;
        public ThemeControl()
        {
            SetStyle((ControlStyles)8198, true);
            B = new Bitmap(1, 1);
            G = Graphics.FromImage(B);
        }

        public void AllowTransparent()
        {
            SetStyle(ControlStyles.Opaque, false);
            SetStyle((ControlStyles)141314, true);
        }

        #endregion

        #region " Mouse Handling "

        public enum State : byte
        {
            MouseNone = 0,
            MouseOver = 1,
            MouseDown = 2
        }

        protected State MouseState;
        protected override void OnMouseLeave(EventArgs e)
        {
            ChangeMouseState(State.MouseNone);
            base.OnMouseLeave(e);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            ChangeMouseState(State.MouseOver);
            base.OnMouseEnter(e);

        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            ChangeMouseState(State.MouseOver);
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ChangeMouseState(State.MouseDown);
            base.OnMouseDown(e);
        }

        private void ChangeMouseState(State e)
        {
            MouseState = e;
            Invalidate();
        }

        #endregion

        #region " Sizing "

        protected override void OnSizeChanged(EventArgs e)
        {
            G.Dispose();
            B.Dispose();
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);
            Invalidate();
            base.OnSizeChanged(e);
        }

        #endregion

        #region " Convienence "

        protected override abstract void OnPaint(PaintEventArgs e);

        public void DrawCorners(Color c, Rectangle rect)
        {
            B.SetPixel(rect.X, rect.Y, c);
            B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c);
            B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c);
            B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c);
        }

        public void DrawBorders(Pen p1, Pen p2, Rectangle rect)
        {
            G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
        }

        private Size TextSize;
        public void DrawText(HorizontalAlignment a, Brush b, int offset = 0)
        {
            if (string.IsNullOrEmpty(Text))
                return;
            TextSize = G.MeasureString(Text, Font).ToSize();

            switch (a)
            {
                case HorizontalAlignment.Left:
                    G.DrawString(Text, Font, b, 5 + offset, Height / 2 - TextSize.Height / 2);
                    break;
                case HorizontalAlignment.Right:
                    G.DrawString(Text, Font, b, Width - 5 - TextSize.Width - offset, Height / 2 - TextSize.Height / 2);
                    break;
                case HorizontalAlignment.Center:
                    G.DrawString(Text, Font, b, Width / 2 - TextSize.Width / 2, Height / 2 - TextSize.Height / 2);
                    break;
            }
        }

        public int ImageWidth
        {
            get
            {
                if (_Image == null)
                    return 0;
                return _Image.Width;
            }
        }

        private Image _Image;
        public Image Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                Invalidate();
            }
        }

        public void DrawIcon(HorizontalAlignment a, int offset = 0)
        {
            if (_Image == null)
                return;
            switch (a)
            {
                case HorizontalAlignment.Left:
                    G.DrawImage(_Image, Width / 10 + offset, Height / 2 - _Image.Height / 2);
                    break;
                case HorizontalAlignment.Right:
                    G.DrawImage(_Image, Width - (Width / 10) - TextSize.Width - offset, Height / 2 - TextSize.Height / 2);
                    break;
                case HorizontalAlignment.Center:
                    G.DrawImage(_Image, Width / 2 - TextSize.Width / 2, Height / 2 - TextSize.Height / 2);
                    break;
            }
        }

        #endregion

    }

    class GTheme : Theme
    {

        public GTheme()
        {
            MoveHeight = 28;
            ForeColor = Color.FromArgb(100, 100, 100);
            SetTransparent(Color.Fuchsia);

            C1 = Color.FromArgb(41, 41, 41);
            C2 = Color.FromArgb(25, 25, 25);

            P1 = new Pen(Color.FromArgb(58, 58, 58));
            P2 = new Pen(C2);
        }

        private Color C1;
        private Color C2;
        private Pen P1;
        private Pen P2;
        private LinearGradientBrush B1;

        private Rectangle R1;
        protected override void OnPaint(PaintEventArgs e)
        {
            G.Clear(C1);

            R1 = new Rectangle(0, 0, Width, 28);
            B1 = new LinearGradientBrush(R1, C2, C1, LinearGradientMode.Vertical);
            G.FillRectangle(B1, R1);

            G.DrawLine(P2, 0, 28, Width, 28);
            G.DrawLine(P1, 0, 29, Width, 29);

            DrawText(HorizontalAlignment.Left, new SolidBrush(ForeColor), ImageWidth);
            DrawIcon(HorizontalAlignment.Left);

            DrawBorders(Pens.Black, P1, ClientRectangle);
            DrawCorners(Color.Fuchsia, ClientRectangle);

            e.Graphics.DrawImage(B, 0, 0);
        }
    }
    class GButton : ThemeControl
    {

        private Pen P1;
        private Pen P2;
        private LinearGradientBrush B1;
        private Color C1;
        private Color C2;

      //  private Rectangle R1;
        public GButton()
        {
            AllowTransparent();
            BackColor = Color.FromArgb(41, 41, 41);
            ForeColor = Color.FromArgb(100, 100, 100);

            P1 = new Pen(Color.FromArgb(25, 25, 25));
            P2 = new Pen(Color.FromArgb(11, Color.White));

            C1 = Color.FromArgb(41, 41, 41);
            C2 = Color.FromArgb(51, 51, 51);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (MouseState == State.MouseDown)
            {
                B1 = new LinearGradientBrush(ClientRectangle, C1, C2, LinearGradientMode.Vertical);
            }
            else
            {
                B1 = new LinearGradientBrush(ClientRectangle, C2, C1, LinearGradientMode.Vertical);
            }

            G.FillRectangle(B1, ClientRectangle);

            DrawText(HorizontalAlignment.Center, new SolidBrush(ForeColor));
            DrawIcon(HorizontalAlignment.Left);

            DrawBorders(P1, P2, ClientRectangle);
            DrawCorners(BackColor, ClientRectangle);

            e.Graphics.DrawImage(B, 0, 0);
        }
    }
    class Seperator : ThemeControl
    {

        public Seperator()
        {
            AllowTransparent();
            BackColor = Color.Transparent;
        }

        private Orientation _Direction;
        public Orientation Direction
        {
            get { return _Direction; }
            set
            {
                _Direction = value;
                Invalidate();
            }
        }

        private Color _Color1 = Color.FromArgb(90, Color.Black);
        public Color Color1
        {
            get { return _Color1; }
            set
            {
                _Color1 = value;
                Invalidate();
            }
        }

        private Color _Color2 = Color.FromArgb(14, Color.White);
        public Color Color2
        {
            get { return _Color2; }
            set
            {
                _Color2 = value;
                Invalidate();
            }
        }

      //  private Rectangle R1;
      //  private LinearGradientBrush B1;

     //   private int Rotation;
        protected override void OnPaint(PaintEventArgs e)
        {
            G.Clear(BackColor);

            if (_Direction == Orientation.Horizontal)
            {
                G.DrawLine(new Pen(_Color1), 0, Height / 2, Width, Height / 2);
                G.DrawLine(new Pen(_Color2), 0, Height / 2 + 1, Width, Height / 2 + 1);
            }
            else
            {
                G.DrawLine(new Pen(_Color1), Width / 2, 0, Width / 2, Height);
                G.DrawLine(new Pen(_Color2), Width / 2 + 1, 0, Width / 2 + 1, Height);
            }

            e.Graphics.DrawImage(B, 0, 0);
        }
    }



}
