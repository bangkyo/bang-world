using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SystemControlClassLibrary.monitoring;

namespace SystemControlClassLibrary
{
    partial class Line3WholeTrk_Working : Line3WholeTrk
    {
        public Line3WholeTrk_Working(string titleNm, string scrAuth, string factCode, string ownerNm) :base(titleNm, scrAuth, factCode, ownerNm)
        {
            
            InitializeComponent();
            //base.InitializeComponent();
            this.Load += Line3WholeTrk_Working_Load;
        }

        private void Line3WholeTrk_Working_Load(object sender, EventArgs e)
        {
            #region Points Make
            List<Point> pointList = new List<Point>();

            pointList.Add(new Point(0, 0));
            pointList.Add(new Point(0, -225));
            pointList.Add(new Point(100, -225));

            pointList.Add(new Point(169, -225));
            pointList.Add(new Point(169, -80));
            pointList.Add(new Point(321, -80));
            pointList.Add(new Point(321, -348));
            pointList.Add(new Point(478, -348));
            pointList.Add(new Point(478, -142));
            pointList.Add(new Point(729, -142));

            pointList.Add(new Point(729, -78));
            pointList.Add(new Point(900, -78));
            pointList.Add(new Point(900, -340));
            pointList.Add(new Point(1057, -340));
            pointList.Add(new Point(1057, -125));

            List<Point> p3Z20_List = new List<Point>();
            p3Z20_List.Add(new Point(478, -348));
            p3Z20_List.Add(new Point(729, -348));
            p3Z20_List.Add(new Point(729, -145));

            List<Point> p3Z19_List = new List<Point>();
            p3Z19_List.Add(new Point(478, -348));
            p3Z19_List.Add(new Point(812, -348));
            p3Z19_List.Add(new Point(812, -80));


            List<Point> p3Z25_List = new List<Point>();
            p3Z25_List.Add(new Point(571, -143));
            p3Z25_List.Add(new Point(571, -205));

            List<Point> p3Z24_List = new List<Point>();
            p3Z24_List.Add(new Point(571, -143));
            p3Z24_List.Add(new Point(571, -100));


            //3Z28
            List<Point> p3Z28_List = new List<Point>();
            p3Z28_List.Add(new Point(643, -222));
            p3Z28_List.Add(new Point(643, -145));

            //3Z27
            List<Point> p3Z27_List = new List<Point>();
            p3Z27_List.Add(new Point(643, -80));
            p3Z27_List.Add(new Point(643, -139));

            points = MakePointList(startpointX, startpointY, pointList).ToArray();
            p3Z20_points = MakePointList(startpointX, startpointY, p3Z20_List).ToArray();
            p3Z19_points = MakePointList(startpointX, startpointY, p3Z19_List).ToArray();
            p3Z25_points = MakePointList(startpointX, startpointY, p3Z25_List).ToArray();
            p3Z24_points = MakePointList(startpointX, startpointY, p3Z24_List).ToArray();
            p3Z28_points = MakePointList(startpointX, startpointY, p3Z28_List).ToArray();
            p3Z27_points = MakePointList(startpointX, startpointY, p3Z27_List).ToArray();
            #endregion
        }

        static List<Point> MakePointList(int _startpointX, int _startpointY, List<Point> _pointList)
        {

            List<Point> RealpointList = new List<Point>();


            foreach (Point item in _pointList)
            {
                RealpointList.Add(new Point(item.X + _startpointX, item.Y + _startpointY));
            }

            return RealpointList;
        }

        protected override void UC_Zone_Setup()
        {
            //base.UC_Zone_Setup();
            List<string> hideZoneList = new List<string>();

            hideZoneList.Add("3Z02");
            hideZoneList.Add("3Z13");
            hideZoneList.Add("3Z14");
            hideZoneList.Add("3Z16");
            hideZoneList.Add("3Z17");
            hideZoneList.Add("3Z18");
            hideZoneList.Add("3Z22");
            hideZoneList.Add("3Z23");
            hideZoneList.Add("3Z26");
            hideZoneList.Add("3Z29");
            hideZoneList.Add("3Z31");
            hideZoneList.Add("3Z32");

            foreach (var item in vf.GetAllChildrens(this))
            {
                if (item.GetType().ToString() == "WindowsFormsApplication15.UC_Zone")
                {
                    var uc_zone = item as WindowsFormsApplication15.UC_Zone;
                    if (CanZoneMoveAcl)
                    {
                        uc_zone.PopupEvent += PopupEvent;
                    }

                    foreach (var zone in hideZoneList)
                    {
                        if (uc_zone.ZoneCD == zone)
                        {
                            uc_zone.Visible = false;
                            this.Invalidate();
                        }
                    }


                }
            }

        }

        const int lineSize = 8;
        int startpointX = 115;
        int startpointY = 450;
        Point[] p3Z25_points;
        Point[] p3Z28_points;

        Point[] points;
        Point[] p3Z20_points;
        Point[] p3Z19_points;
        Color lineColor = Color.FromArgb(0, 122, 204);
        private Point[] p3Z27_points;
        private Point[] p3Z24_points;

        protected override void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //base.pictureBox1_Paint(sender, e);

            PictureBox pb = (PictureBox)sender;

            Graphics g = e.Graphics;

            using (Pen gridLineColor = new Pen(lineColor, lineSize))
            {
                DrawLines(g, gridLineColor, points);
            }

            using (Pen arrowPen = new Pen(lineColor, lineSize))
            {
                arrowPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                DrawLines(g, arrowPen, p3Z20_points);
                DrawLines(g, arrowPen, p3Z19_points);

                DrawLines(g, arrowPen, p3Z25_points);
                DrawLines(g, arrowPen, p3Z24_points);

                DrawLines(g, arrowPen, p3Z28_points);
                DrawLines(g, arrowPen, p3Z27_points);
            }
        }

        private void DrawLines(Graphics g, Pen pen, Point[] _points)
        {
            g.DrawLines(pen, _points);
        }
    }
}
