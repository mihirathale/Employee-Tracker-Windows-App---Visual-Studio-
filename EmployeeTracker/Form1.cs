using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Data.SqlClient;
using GoogleMaps.LocationServices;

namespace EmployeeTracker
{
    public partial class Form1 : Form
    {
        GMapOverlay markers = new GMapOverlay("markers");
        GMapRoute line_layer;
       
        double latd, longt;
        string usrid;
        string user, time;
        public Form1()
        {
            InitializeComponent();
            fillCombo();
            
        }
        void fillCombo() {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "data source = TECH-PC3;database=PRESALES;integrated security =false;UID =sa ; Password = oispl ";
            cn.Open();
            SqlCommand cmmd = new SqlCommand("Select distinct U_UserName,U_UserCode from [dbo].[@OMTR]", cn);
            SqlDataReader rdr = cmmd.ExecuteReader();
            while (rdr.Read())
            {
                usrid = rdr["U_UserName"].ToString();
                comboBox1.Items.Add(usrid);
            }
            cn.Close();
        }
        void name() {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "data source = TECH-PC3;database=PRESALES;integrated security =false;UID =sa ; Password = oispl ";
            conn.Open();
            SqlCommand cmmd = new SqlCommand("Select U_UserCode from [dbo].[@OMTR] where U_UserName='" + comboBox1.Text + "'", conn);
            SqlDataReader rdr = cmmd.ExecuteReader();
            while (rdr.Read())
            {
                usrnm.Text = rdr["U_UserCode"].ToString();
                
            }
            conn.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            map.ShowCenter = false;
        }

        public void button1_Click_1(object sender, EventArgs e)
        {
            name();
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleHybridMap;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "data source = TECH-PC3;database=PRESALES;integrated security =false;UID =sa ; Password = oispl ";
            con.Open();
            SqlCommand cmmd = new SqlCommand("Select * from [dbo].[@MTR1] INNER JOIN [dbo].[@OMTR] ON [dbo].[@MTR1].DocEntry = [dbo].[@OMTR].DocEntry WHERE [dbo].[@MTR1].U_DateTime='"+dateTimePicker1.Value.Date.ToString("yyyy-MM-dd ")+"00:00:00.000"+ "' AND [dbo].[@OMTR].U_UserCode='"+ usrnm.Text +"' ", con);
            SqlDataReader rdr = cmmd.ExecuteReader();
            line_layer = new GMapRoute("single_line");
            line_layer.Stroke = new Pen(Brushes.GreenYellow, 2);
            markers.Routes.Add(line_layer);
            if (rdr.Read())
            {
                latd = Convert.ToDouble(rdr["U_Lat"].ToString());
                longt = Convert.ToDouble(rdr["U_Long"].ToString());

                user = rdr["U_UserName"].ToString();
                time = rdr["U_Time"].ToString();
                PointLatLng point = new PointLatLng(latd, longt);
                RoutingProvider routingProvider = map.MapProvider as RoutingProvider ?? GMapProviders.OpenStreetMap;
                map.Position = point;
                line_layer.Points.Add(point);
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.green_pushpin);
                var location = new GoogleLocationService();
                string address = Convert.ToString(location.GetAddressFromLatLang(latd, longt));
                marker.ToolTipText = address + "\n" + user + "\n" + time;
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                markers.Markers.Add(marker);
                map.UpdateRouteLocalPosition(line_layer);


            }
            while (rdr.Read())
             {
                latd = Convert.ToDouble(rdr["U_Lat"].ToString());
                longt = Convert.ToDouble(rdr["U_Long"].ToString());
                
                user = rdr["U_UserName"].ToString();
                time = rdr["U_Time"].ToString();
                PointLatLng point = new PointLatLng(latd, longt);
                RoutingProvider routingProvider = map.MapProvider as RoutingProvider ?? GMapProviders.OpenStreetMap;
                map.Position = point;
                line_layer.Points.Add(point);
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
                var location = new GoogleLocationService();
                string address = Convert.ToString(location.GetAddressFromLatLang(latd, longt));
                marker.ToolTipText = address +"\n"+user+"\n"+time;
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                markers.Markers.Add(marker);
                map.UpdateRouteLocalPosition(line_layer);


             }
            con.Close();
            map.Overlays.Add(markers);
            map.MinZoom = 0;
            map.MaxZoom = 500;
            map.Zoom = 10;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            markers.Markers.Clear();
            markers.Routes.Clear();
            comboBox1.Text = "";
            usrnm.Text = "";
            dateTimePicker1.ResetText();
        }

        
    }
}
