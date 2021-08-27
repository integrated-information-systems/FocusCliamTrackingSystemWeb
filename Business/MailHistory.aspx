<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="MailHistory.aspx.vb" Inherits="Business_MailHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 54px;
        }
        .style3
        {
            width: 35px;
        }
    </style>
      <link href="../Styles/BubbleStyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Email Thread</h1>
   <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" ><asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Green"></asp:Label> </asp:Panel>

    <table class="style1">
        <tr>
            <td class="style2">
                Track Id:</td>
            <td class="style3">
              <asp:Label runat="server" ID="lblTrackingId" ></asp:Label>
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" Visible="False" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <%--<div class="contentBubble">--%>
    <asp:Panel runat="server" ID="container" >
<%--<div class="contentBubble">--%>
<asp:Panel runat="server" ID="BubbleContent" CssClass="contentBubble">
<asp:Literal ID="ContentHolder" runat="server"></asp:Literal>
<%--    <blockquote class="example-right">
      <p>Dear Mr. Williams:</p>
<p>Thank you for inquiring about our new email marketing enterprise application. A team member will contact you tomorrow with a detailed explanation of the product that fits your business need.</p>
<p>Thanks again for your inquiry.</p>
<p>Sincerely,</p>
<p>James Burton</p>
      </blockquote>
      <p>Ivan Chermayeff</p>
    
     <blockquote class="example-right-odd">
       <p>Dear Mr. Gate:</p>
<p>Thank you for your order of 25 DVDs. We will send them within the next 3 days.</p>
<p>Before we send them however, we need to know the type of package you prefer. Kindly visit your order page and select your preference. If you have any question, call us at +2348035290896. You will be promptly attended to by the customer service team.</p>
<p>Thanks again for your order. We look forward to your final instructions.</p>
<p>Sincerely,</p>
<p>James Noah</p>
      </blockquote>
      <p>Ivan Chermayeff</p>--%>
    </asp:Panel>
<%--   </div>--%>
  <%--  </div>--%>
  </asp:Panel>
</asp:Content>

