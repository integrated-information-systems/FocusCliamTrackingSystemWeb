<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ClaimByStatus.aspx.vb" Inherits="Reports_ClaimByStatus" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 114px;
        }
    </style>
    <script src="../crystalreportviewers13/js/crviewer/crv.js" type="text/javascript" language="javascript" ></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Claim list by status</h1>
<table class="style1">
    <tr>
        <td class="style2">
            Claim Status</td>
        <td>
         <asp:DropDownList ID="ddlStatus" runat="server">                              
                                <asp:ListItem Selected="True">Open</asp:ListItem>                               
                                <asp:ListItem>Complete</asp:ListItem>
                                </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="style2">
            &nbsp;</td>
        <td>
            <asp:Button ID="btnSubmit" runat="server" Text="Search" />
        </td>
    </tr>
    </table>
    <center>
     <CR:CrystalReportViewer ID="CrystalReportViewer1" 
                          runat="server" AutoDataBind="True"                
                          ReportSourceID="CrystalReportSource1"  
                      />
  <CR:CrystalReportSource ID="CrystalReportSource1"  
                          runat="server">
            <Report FileName="~/Reports/ClaimByStatus.rpt">
            </Report>
   </CR:CrystalReportSource>
   </center>
</asp:Content>

