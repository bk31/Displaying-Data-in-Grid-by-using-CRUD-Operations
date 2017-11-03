using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    string strcon = ConfigurationManager.ConnectionStrings["A"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        if(!IsPostBack)
        {
            AmmuInfo();
        }
    }
    private void AmmuInfo()
    {
        using (SqlConnection cn = new SqlConnection(strcon))
        {
            using (SqlCommand cmd = new SqlCommand("select * from Ammu", cn))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    gvAmmu.DataSource = dt;
                    gvAmmu.DataBind();
                    Session["ds"] = ds;
                }
            }
        }
    }

    protected void gvAmmu_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvAmmu.EditIndex = e.NewEditIndex;
        DataSet ds = Session["ds"] as DataSet;
        DataTable dt = ds.Tables[0];
        gvAmmu.DataSource = dt;
        gvAmmu.DataBind();
        //Session["ds"] = ds;

    }

    protected void gvAmmu_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvAmmu.EditIndex = -1;
        e.Cancel = true;
        DataSet ds = Session["ds"] as DataSet;
        DataTable dt = ds.Tables["Ammu"];
        gvAmmu.DataSource = dt;
        gvAmmu.DataBind();
        AmmuInfo();
    }



    protected void gvAmmu_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int row = e.RowIndex;
        GridViewRow gvr = gvAmmu.Rows[row];
        Label lblAid = gvr.FindControl("lblAid") as Label;
        TextBox tName = gvr.FindControl("tbName") as TextBox;
        TextBox tAge = gvr.FindControl("tbAge") as TextBox;
        int Aid = Convert.ToInt32(lblAid.Text);
        string Name = tName.Text;
        int Age = Convert.ToInt32(tAge.Text);
        string qry = string.Format("update Ammu set Name='{0}',Age={1} where Aid='{2}'", Name, Age, Aid);
        SqlConnection cn = new SqlConnection( strcon);
        SqlCommand cmd = new SqlCommand(qry,cn);
        cn.Open();
        cmd.ExecuteNonQuery();
        cn.Close();

    }
}