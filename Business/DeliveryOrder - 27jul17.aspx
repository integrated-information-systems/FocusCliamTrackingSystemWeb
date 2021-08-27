<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="DeliveryOrder.aspx.vb" Inherits="Business_DeliveryOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 121px;
        }
        .style3
        {
            text-align: right;
            width: 24%;
        }
        .style4
        {
        }
    </style>
       <link href="../Styles/jquery-ui.css" 
            rel="stylesheet" type="text/css"/>       
       <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript" language="javascript" ></script>
       <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Scripts/JScript.js" type="text/javascript" language="javascript" ></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Delivery Order</h1>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center"><asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Green"></asp:Label>  </asp:Panel>
    <table class="style1">        
        <tr>
            <td>
                <table class="style1">
                    <tr>
                        <td class="style2">
                            End Customer Name</td>
                        <td>
                            <asp:TextBox ID="txtEndCustomerName" runat="server" Width="457px" 
                                MaxLength="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorEndCustomerName" ForeColor="Red" ControlToValidate="txtEndCustomerName" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            Person In charge</td>
                        <td>
                            <asp:TextBox ID="txtPersonincharge" runat="server" Width="457px" 
                                MaxLength="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorPersonincharge" ForeColor="Red" ControlToValidate="txtPersonincharge" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            Email Address</td>
                        <td>
                            <asp:TextBox ID="txtEmailAddress" runat="server" Width="457px" MaxLength="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmailAddress" ForeColor="Red" ControlToValidate="txtEmailAddress" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            Tel No</td>
                        <td>
                            <asp:TextBox ID="txtTelno" runat="server" Width="194px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorTelno" ForeColor="Red" ControlToValidate="txtTelno" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorTelno" ForeColor="Red" ControlToValidate="txtTelno" runat="server" ErrorMessage="*"   ValidationExpression="\d{1,}" ValidationGroup="SubmitForm" >*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Brand</td>
                        <td>
                            <asp:TextBox ID="txtBrand" runat="server" MaxLength="25"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Machine Model No</td>
                        <td>
                            <asp:TextBox ID="txtMachineModelNo" runat="server" MaxLength="25"></asp:TextBox></td>
                    </tr>  
                </table>
            </td>
            <td>
                <table class="style1">
                    <tr>
                        <td style="text-align: right">
                            <asp:TextBox ID="txtFind" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnFind" runat="server" Text="Find" /><%--<asp:Button ID="btnNew" runat="server" Text="New" /><asp:Button ID="btnDuplicate" runat="server" Text="Duplicate" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Claim ID</td>
                        <td>
                            <asp:TextBox ID="txtClaimId" runat="server" ReadOnly="True" Wrap="False" 
                                style="text-align:right" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            DO ID</td>
                        <td>
                            <asp:TextBox ID="txtDOId" runat="server" ReadOnly="True" Wrap="False" 
                                style="text-align:right" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date</td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" ReadOnly="True" 
                                style="text-align:left" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Country</td>
                        <td>
                            <asp:TextBox ID="txtCountry" runat="server" ReadOnly="True" style="text-align:left" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Status</td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false">                              
                                <asp:ListItem>Open</asp:ListItem>
                                <asp:ListItem>Closed</asp:ListItem>
                                <asp:ListItem>Cancelled</asp:ListItem>
                                <asp:ListItem>Complete</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPriority" runat="server" Text="Priority"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddlPriority" runat="server">
                                <asp:ListItem Value="1">High</asp:ListItem>
                                <asp:ListItem Selected="True" Value="0">Low</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>                
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Note: All fields need to be filled before saving. Once saved, details cannot be edited. " ForeColor="Red"></asp:Label>
                <asp:CustomValidator ID="RowCountValidator" runat="server"   ValidationGroup="SubmitForm"  ControlToValidate="txtCountry"  OnServerValidate="RowCountValidator_ServerValidate"               ErrorMessage="*"></asp:CustomValidator>
            </td>
<%--            <td align="right">
            <asp:Button ID="btnAdd" runat="server" Text="Add" />
            <asp:Button ID="btnCopy" runat="server" Text="Copy" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" />
            </td>--%>
            </tr>
        <tr>
            <td colspan="2" align="center">
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldddlPriority" Display="Dynamic" ForeColor="Red" ControlToValidate="txtFind" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>--%>
                      
                <asp:GridView ID="DeliveryItemListGrid" runat="server"   AutoGenerateColumns="false"
        EmptyDataText="No Records Added" ForeColor="#333333" GridLines="None" 
        ShowHeaderWhenEmpty="True" Width="100%"  
        HorizontalAlign="Center" >
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="SNo" >
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" Text='<%# Bind("SNo") %>'></asp:Label>                        
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <asp:Label ID="lblSNo" runat="server" Text='<%# Bind("SNo") %>'></asp:Label>                             
                            </EditItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="Claim SNo" >
                            <ItemTemplate>
                                <asp:Label ID="lblClaimSNo" runat="server" Text='<%# Bind("ClaimSNo") %>'></asp:Label>                        
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <asp:Label ID="lblClaimSNo" runat="server" Text='<%# Bind("ClaimSNo") %>'></asp:Label>                             
                            </EditItemTemplate>
                        </asp:TemplateField>                                          
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvDate" runat="server" Text='<%# Bind("InvDate") %>'></asp:Label>                        
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblInvDate" runat="server" Text='<%# Bind("InvDate") %>'></asp:Label>       
                            </EditItemTemplate>
                        </asp:TemplateField>          
                         <asp:TemplateField HeaderText="Invoice No">
                            <ItemTemplate>
                                <asp:Label ID="lblInvNo" runat="server" Text='<%# Bind("InvNo") %>'></asp:Label>                        
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblInvNo" runat="server" Text='<%# Bind("InvNo") %>'></asp:Label>              
                            </EditItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Qty">
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>                        
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>              
                            </EditItemTemplate>
                        </asp:TemplateField>    
                       <%--  <asp:TemplateField HeaderText="Brand" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="Brand" Text='<%# Bind("Brand") %>'  MaxLength="25">  </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorBrand" ForeColor="Red" ControlToValidate="Brand" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Machine Model No"  Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ModelNo" Text='<%# Bind("ModelNo") %>' MaxLength="25"  >  </asp:TextBox>             
                               <asp:RequiredFieldValidator ID="RequiredFieldValidatorModelNo" ForeColor="Red" ControlToValidate="ModelNo" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>                                                
                            </ItemTemplate>
                        </asp:TemplateField>--%>    
                         <asp:TemplateField HeaderText="Machine Serial No">
                            <ItemTemplate>
                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SerialNo") %>'></asp:Label>                        
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SerialNo") %>'></asp:Label>            
                            </EditItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Claim Qty">
                            <ItemTemplate>
                                <asp:Label ID="lblClaimQty" runat="server" Text='<%# Bind("ClaimQty") %>'></asp:Label>                        
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblClaimQty" runat="server" Text='<%# Bind("ClaimQty") %>'></asp:Label>          
                            </EditItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="DO Qty">
                            <ItemTemplate>
                                <asp:Label ID="lblDOQty" runat="server" Text='<%# Bind("DOQty") %>'></asp:Label>                        
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" id="DOQty" Text='<%# Bind("DOQty") %>' Width="40px"  MaxLength="25"  > </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDOQty" ForeColor="Red" ControlToValidate="DOQty" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDOQty" ForeColor="Red" ControlToValidate="DOQty" runat="server" ErrorMessage="*"   ValidationExpression="\d{1,}" ValidationGroup="SubmitForm" >*</asp:RegularExpressionValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Part no/ Description">
                            <ItemTemplate>
                                <asp:Label ID="lblParticulars" runat="server" Text='<%# Bind("Particulars") %>'></asp:Label>                        
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblParticulars" runat="server" Text='<%# Bind("Particulars") %>'></asp:Label>  
                            </EditItemTemplate>
                        </asp:TemplateField>    
                          <asp:TemplateField HeaderText="Claim Mode"  Visible="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblClaimmode" runat="server" Text='<%# Bind("Claimmode") %>'></asp:Label>                        
                            </ItemTemplate>                           
                            <EditItemTemplate>
                                <asp:Label ID="lblClaimmode" runat="server" Text='<%# Bind("Claimmode") %>'></asp:Label>                                
                            </EditItemTemplate>
                        </asp:TemplateField>   
                          <asp:TemplateField HeaderText="Claim Date" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblClaimDate" runat="server" Text='<%# Bind("ClaimDate") %>'></asp:Label>                        
                            </ItemTemplate>  
                            <EditItemTemplate>
                                <asp:Label ID="lblClaimDate" runat="server" Text='<%# Bind("ClaimDate") %>'></asp:Label>                                    
                            </EditItemTemplate>
                        </asp:TemplateField>                                                       
                          <asp:TemplateField HeaderText="Claim Reference" Visible="false">
                          <ItemTemplate>
                                <asp:Label ID="lblClaimReference" runat="server" Text='<%# Bind("ClaimReference") %>'></asp:Label>                        
                            </ItemTemplate>  
                            <EditItemTemplate>
                                <asp:Label ID="lblClaimReference" runat="server" Text='<%# Bind("ClaimReference") %>'></asp:Label>                                 
                            </EditItemTemplate>
                        </asp:TemplateField>   
                          <asp:TemplateField HeaderText="Part No" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPartNo" runat="server" Text='<%# Bind("PartNo") %>'></asp:Label>                        
                            </ItemTemplate>  
                            <EditItemTemplate>
                                <asp:Label ID="lblPartNo" runat="server" Text='<%# Bind("PartNo") %>'></asp:Label>                                     
                            </EditItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Declare Value"  >
                            <ItemTemplate>
                                <asp:Label ID="lblDeclareValue" runat="server" Text='<%# Bind("DeclareValue") %>'></asp:Label>                        
                            </ItemTemplate> 
                            <EditItemTemplate>
                                <asp:Label ID="lblDeclareValue" runat="server" Text='<%# Bind("DeclareValue") %>'></asp:Label>                               
                            </EditItemTemplate>
                        </asp:TemplateField>   
                        <asp:TemplateField HeaderText="Declare Description"  >
                            <ItemTemplate>
                                <asp:Label ID="lblDeclareDescription" runat="server" Text='<%# Bind("DeclareDescription") %>'></asp:Label>                        
                            </ItemTemplate> 
                            <EditItemTemplate>
                                <asp:Label ID="lblDeclareDescription" runat="server" Text='<%# Bind("DeclareDescription") %>'></asp:Label>                                    
                            </EditItemTemplate>
                        </asp:TemplateField>                            
                          <asp:TemplateField HeaderText="Claimed" Visible="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblClaimed" runat="server" Text='<%# Bind("Claimed") %>'></asp:Label>                        
                            </ItemTemplate> 
                            <EditItemTemplate>
                                <asp:Label runat="server" id="Claimed" Text='<%# Bind("Claimed") %>'  Font-Bold="true" >  </asp:Label>                                    
                            </EditItemTemplate>
                        </asp:TemplateField>       
                           <asp:TemplateField HeaderText="Status" Visible="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>                        
                            </ItemTemplate> 
                            <EditItemTemplate>
                                <asp:Label runat="server" id="Status" Text='<%# Bind("Status") %>'  Font-Bold="true" >  </asp:Label>                                    
                            </EditItemTemplate>
                        </asp:TemplateField>      
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true"   />     
                    </Columns>
    <EditRowStyle BackColor="#999999" />
    <EmptyDataRowStyle BackColor="#00CCFF" />
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
              
            </td>
        </tr>       
    </table>

    <table class="style1">
        <tr>
            <td valign="top">
                <table class="style1">
                    <tr>
                        <td valign="top" class="style4" colspan="2">
                            <%--<asp:Label ID="Label2" runat="server" Text=" For damaged parts claim, please upload photo or video up to 2 mb. For video more than 2 mb, please use video link" ForeColor="Red"></asp:Label>--%>
                            <asp:Label ID="Label3" runat="server" Text=" For damaged parts claim, please upload photo or video, please use video link" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="style4">
                            Upload Photo/Video</td>
                        <td>
                            <asp:FileUpload ID="PhotosUpload" runat="server"    /><asp:Button ID="btnUpload" runat="server"   Text="Upload" OnClientClick = "return ValidateFile()" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblUploadMsg" runat="server" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="style4" >
                            <asp:BulletedList ID="UploadedFiles" runat="server" BulletStyle="Numbered" DisplayMode="LinkButton" CssClass="ImageNamelist"
                                ViewStateMode="Enabled">
                            </asp:BulletedList>                            
                        </td>                      
                        <td valign="top">  <div style="Width:71px;" class="imglist">
                       <asp:Repeater ID="UploadedThumbs" runat="server">                       
                            </asp:Repeater> </div></td>
                    </tr>
                    <tr>
                        <td class="style4" style="text-align: right">
                            <strong>Paste
                            video links</strong></td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style3">
                            1</td>
                        <td>
                            <asp:TextBox ID="txtYoutubeLink1" runat="server" Width="297px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            2</td>
                        <td>
                            <asp:TextBox ID="txtYoutubeLink2" runat="server" Width="297px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            3</td>
                        <td>
                            <asp:TextBox ID="txtYoutubeLink3" runat="server" Width="297px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">
                            
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table class="style1" style="border: 1px solid #DDE4EC;">
                    <tr>
                        <td colspan="2">
                            <strong>For Delivery Purpose</strong><asp:Label ID="Label2" runat="server" Text=" Note:(All Fields are mandatory)" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            Ship To Company</td>
                        <td>
                            <asp:TextBox ID="txtShipToCompany" runat="server" Width="457px" MaxLength="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="txtShipToCompanyRequiredFieldValidator" ForeColor="Red" ControlToValidate="txtShipToCompany" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Ship to Address</td>
                        <td>
                            <asp:TextBox ID="txtShipToAddress" style="resize:none; margin-top: 0px;" 
                                runat="server" Rows="5" TextMode="MultiLine" 
                                Width="261px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtShipToAddressRequiredFieldValidator" ForeColor="Red" ControlToValidate="txtShipToAddress" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contact Person</td>
                        <td>
                            <asp:TextBox ID="txtShipToContactPerson" runat="server" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="txtShipToContactPersonRequiredFieldValidator" ForeColor="Red" ControlToValidate="txtShipToContactPerson" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tel No</td>
                        <td>
                            <asp:TextBox ID="txtShipToTelNo" runat="server" Width="184px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="txtShipToTelNoRequiredFieldValidator" ForeColor="Red" ControlToValidate="txtShipToTelNo" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblreasonforrejection" runat="server" Text="Old Remarks"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtreasonforrejection" runat="server" Rows="5" 
                                TextMode="MultiLine" style="resize:none; margin-top: 0px;" 
                                Width="700px"></asp:TextBox>                            
                        </td>
                    </tr>
		    <tr>
                        <td>
                            <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" Rows="5" 
                                TextMode="MultiLine" style="resize:none; margin-top: 0px;" 
                                Width="700px"></asp:TextBox>
                            <asp:Button ID="BtnAddRemarks" runat="server" Text="Add Remarks" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>

                        <asp:GridView ID="ItemDetailsGrid"
             AllowSorting="False"
             AllowPaging="True" 
             runat="server"
             CellPadding="4"
             ForeColor="#333333"
             AutoGenerateColumns="False"
             ShowHeaderWhenEmpty="True"
             GridLines="None"
             EmptyDataText="No Remarks found"
                PageSize="10"
             >
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                <Columns>
                        
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="True" />          
                    <asp:BoundField DataField="CreatedBy" HeaderText="Added By" ReadOnly="True" />
                    <asp:BoundField DataField="CreatedOn" HeaderText="Added On" ReadOnly="True"   />           
                      
                </Columns>
                </asp:GridView>
               
                <% If ItemDetailsGrid.Rows(0).Cells.Count > 1 Then%>
                <i>You are viewing page
                <%=ItemDetailsGrid.PageIndex + 1%>
                of
                <%=ItemDetailsGrid.PageCount%>
                </i>
                <% End If%>
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return confirm('Are you sure?');"
        ValidationGroup="SubmitForm" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" />
                        <asp:Button ID="btnPreview" runat="server" 
        Text="Preview" />     
        <asp:Button ID="btnCancel" runat="server" 
        Text="Cancel" />     
                        </asp:Content>

