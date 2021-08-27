<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ManufacturerInfo.aspx.vb" Inherits="Business_ManufacturerInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
 <link href="../Styles/jquery-ui.css" 
            rel="stylesheet" type="text/css"/>       
       <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript" language="javascript" ></script>
       <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Scripts/JScript.js" type="text/javascript" language="javascript" ></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Manufacturer</h1>
    <asp:ScriptManager ID="ScriptManager1" runat="server"
EnablePageMethods = "true">
</asp:ScriptManager>
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
                            <asp:TextBox ID="txtBrand" runat="server" MaxLength="25" ReadOnly="True"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Machine Model No</td>
                        <td>
                            <asp:TextBox ID="txtMachineModelNo" runat="server" MaxLength="25" ReadOnly="True"></asp:TextBox></td>
                    </tr>  
                                        <tr>
                        <td>Supplier Name</td>
                        <td>
                            <asp:TextBox ID="txtSupplierName" runat="server" Width="360px" MaxLength="100" AutoPostBack="true"></asp:TextBox>
                            <act:AutoCompleteExtender ServiceMethod="GetSupplierNames"
                        MinimumPrefixLength="1"
                        CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"  
                        TargetControlID="txtSupplierName" CompletionListElementID="txtSupplierNameAutoCompleteContainer"
                        ID="txtSupplierNameAutoCompleteExtender" runat="server" FirstRowSelected = "false"> </act:AutoCompleteExtender>
                        <div id="txtSupplierNameAutoCompleteContainer"></div> 
                            </td>
                    </tr>
                    <tr>
                        <td>Contact Person</td>
                        <td>
                            <asp:TextBox ID="txtContactPerson" runat="server" Width="187px" MaxLength="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Contact Number</td>
                        <td>
                            <asp:TextBox ID="txtContactNumber" runat="server" Width="188px" MaxLength="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Supplier Email</td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Width="188px" MaxLength="100"></asp:TextBox></td>
                    </tr>
                </table>
            </td>
            <td >
                <table class="style1">
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
                            <asp:DropDownList ID="ddlStatus" runat="server">                                
                                <asp:ListItem Selected="True">Open</asp:ListItem>
                                <asp:ListItem>Inprocess</asp:ListItem>
                                <asp:ListItem>Complete</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
            <table>
                <tr>
                    <td>No:</td>
                    <td>Date:</td>
                    <td>Action:</td>
                </tr>
                <tr>
                    <td><asp:Label ID="No1" runat="server" Text="1"></asp:Label></td>
                    <td><asp:TextBox ID="txtActionDate1" runat="server" MaxLength="10"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtActionInfo1" runat="server" MaxLength="250"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="No2" runat="server" Text="2"></asp:Label></td>
                    <td><asp:TextBox ID="txtActionDate2" runat="server" MaxLength="10"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtActionInfo2" runat="server" MaxLength="250"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="No3" runat="server" Text="3"></asp:Label></td>
                    <td><asp:TextBox ID="txtActionDate3" runat="server" MaxLength="10"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtActionInfo3" runat="server" MaxLength="250"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="No4" runat="server" Text="4"></asp:Label></td>
                    <td><asp:TextBox ID="txtActionDate4" runat="server" MaxLength="10"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtActionInfo4" runat="server" MaxLength="250"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="No5" runat="server" Text="5"></asp:Label></td>
                    <td><asp:TextBox ID="txtActionDate5" runat="server" MaxLength="10"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtActionInfo5" runat="server" MaxLength="250"></asp:TextBox></td>
                </tr>
                </table>
            </td>
        </tr>
        <tr><td colspan="2" align="right">
            <asp:Button ID="btnAdd" runat="server" Text="Add" Visible="False" />
            <asp:Button ID="btnCopy" runat="server" Text="Copy" Visible="False" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" Visible="False" />
            </td></tr>
        <tr>
            <td colspan="2" align="center">
                <asp:GridView ID="ClaimItemList" runat="server" CellPadding="4"  AutoGenerateColumns="false"
        EmptyDataText="No Records Added" ForeColor="#333333" GridLines="None" 
        ShowHeaderWhenEmpty="True" Width="100%" AutoGenerateSelectButton="True" 
        HorizontalAlign="Center" ViewStateMode="Enabled"  >
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="SNo" >
                            <ItemTemplate>
                                <asp:TextBox runat="server" Width="40px" id="SNo" Text='<%# Bind("SNo") %>'  ReadOnly="true" >  </asp:TextBox>                        
                            </ItemTemplate>
                        </asp:TemplateField>                                          
                        <asp:TemplateField HeaderText="Invoice Date" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="InvDate" Text='<%# Bind("InvDate") %>'  >  </asp:TextBox> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorInvDate" Display="Dynamic" ForeColor="Red" ControlToValidate="InvDate" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>          
                         <asp:TemplateField HeaderText="Invoice No" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="InvNo" Text='<%# Bind("InvNo") %>'  >  </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorInvNo" ForeColor="Red" ControlToValidate="InvNo" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Qty" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="Qty" Text='<%# Bind("Qty") %>' >  </asp:TextBox>              
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorQty" ForeColor="Red" ControlToValidate="Qty" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorQty" ForeColor="Red" ControlToValidate="Qty" runat="server" ErrorMessage="*"   ValidationExpression="\d{1,}" ValidationGroup="SubmitForm" >*</asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <%-- <asp:TemplateField HeaderText="Brand">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="Brand" Text='<%# Bind("Brand") %>'  >  </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorBrand" ForeColor="Red" ControlToValidate="Brand" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Machine Model No">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ModelNo" Text='<%# Bind("ModelNo") %>'  >  </asp:TextBox>             
                               <asp:RequiredFieldValidator ID="RequiredFieldValidatorModelNo" ForeColor="Red" ControlToValidate="ModelNo" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>                     
                            </ItemTemplate>
                        </asp:TemplateField> --%>   
                         <asp:TemplateField HeaderText="Machine Serial No">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="SerialNo" Text='<%# Bind("SerialNo") %>'  >  </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorSerialNo" ForeColor="Red" ControlToValidate="SerialNo" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>                     
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Claim Qty">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ClaimQty" Text='<%# Bind("ClaimQty") %>'>  </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorClaimQty" ForeColor="Red" ControlToValidate="ClaimQty" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorClaimQty" ForeColor="Red" ControlToValidate="ClaimQty" runat="server" ErrorMessage="*"   ValidationExpression="\d{1,}" ValidationGroup="SubmitForm" >*</asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="Particulars" Text='<%# Bind("Particulars") %>'  >  </asp:TextBox>    
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorParticulars" ForeColor="Red" ControlToValidate="Particulars" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>                                         
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Declare Value"  >
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="DeclareValue" Text='<%# Bind("DeclareValue") %>'  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:TemplateField HeaderText="Declare Description"  >
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="DeclareDescription" Text='<%# Bind("DeclareDescription") %>'  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>     
                          <asp:TemplateField HeaderText="Claim Mode"  Visible="false" >
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="Claimmode" Text='<%# Bind("Claimmode") %>'  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>   
                          <asp:TemplateField HeaderText="Claim Date" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ClaimDate" Text='<%# Bind("ClaimDate") %>'  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>   
                          <asp:TemplateField HeaderText="Claim Reference" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ClaimReference" Text='<%# Bind("ClaimReference") %>'  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>   
                          <asp:TemplateField HeaderText="Part No" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="PartNo" Text='<%# Bind("PartNo") %>'  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>                             
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
                        <td valign="top">
                            Upload Photo</td>
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
                        <td colspan="2">
                            <asp:BulletedList ID="UploadedFiles" runat="server" BulletStyle="Numbered" DisplayMode="LinkButton"
                                ViewStateMode="Enabled">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            Youtube Links</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">
                            1</td>
                        <td>
                            <asp:TextBox ID="txtyoutubelink1" runat="server" Width="297px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            2</td>
                        <td>
                            <asp:TextBox ID="txtyoutubelink2" runat="server" Width="297px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            3</td>
                        <td>
                            <asp:TextBox ID="txtyoutubelink3" runat="server" Width="297px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
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
                            <strong>For Delivery Purpose</strong></td>
                    </tr>
                    <tr>
                        <td>
                            Ship To Company</td>
                        <td>
                            <asp:TextBox ID="txtShipToCompany" runat="server" Width="457px" MaxLength="250"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Ship to Address</td>
                        <td>
                            <asp:TextBox ID="txtShipToAddress" style="resize:none; margin-top: 0px;" 
                                runat="server" Rows="5" TextMode="MultiLine" 
                                Width="261px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contact Person</td>
                        <td>
                            <asp:TextBox ID="txtShipToContactPerson" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tel No</td>
                        <td>
                            <asp:TextBox ID="txtShipToTelNo" runat="server" Width="184px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    </table>
            </td>
        </tr>
                
    </table>    
    <table>
    <tr>
    <td valign="top">Remarks</td><td><asp:TextBox ID="txtRemarks" runat="server" style="resize:none; margin-top: 0px;" 
            MaxLength="100" Rows="5" TextMode="MultiLine" Width="312px"></asp:TextBox></td><td>
            &nbsp;</td></tr>
        <tr>
    <td valign="top">Contacted Date</td><td>
            <asp:TextBox ID="txtContactedDate" runat="server"></asp:TextBox><asp:CustomValidator  id="ContactedDateValidator" runat="server"  ValidationGroup="SubmitForm" OnServerValidate="ValidateContactedDate"   ErrorMessage="From date is not valid" Display="Dynamic">*
                        </asp:CustomValidator> 
            </td><td>&nbsp;</td></tr>
    </table>
    <asp:Button ID="btnSave" runat="server" Text="Save" Visible="true"
        ValidationGroup="SubmitForm" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" Visible="false" />
                        <asp:Button ID="btnExporttoExcel" runat="server" 
        Text="Export to Excel" />
</asp:Content>

