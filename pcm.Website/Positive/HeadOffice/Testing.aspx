<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="Testing.aspx.vb" Inherits="pcm.Website.Testing" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>
    <script type="text/javascript" src="../../jquery/Testing.js"></script>
    <style type="text/css"> 
        .date_panel {
            position: relative;
            top: -6px;
            left: -14px;
        }
        .button {
            position: absolute;
            right: 10px;
            bottom: 10px;
        }
    </style>
    <script type="text/javascript">

        function onEnd(s, e) {
            lp.Hide();
        }
        function addRow(s, e) {
            debugger
            e.processOnServer = false;
            var Employeetext = EmployeeNumber.GetText()
            var Firsttext = FirstName.GetText()
            var Lasttext = LastName.GetText()
            var dateofbirth = DateofBirth.GetValue()
            var email = EmailAddress.GetText()
            var male = Male.GetValue()
            var female = Female.GetValue()
            var isValid = false;
            debugger;
            if (Employeetext == "") {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Please Enter the Employee Number")
                ASPxPopupControl1.Show()
                return false;
            }
            var regex = /^[0-9-+()]*$/;
            isValid = regex.test(Employeetext);
            if (isValid == false) {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Please Enter the Integer in Employee Number")
                ASPxPopupControl1.Show()
                return false;
            }
            if (Firsttext == "") {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Please Enter the First Name")
                ASPxPopupControl1.Show()
                return false;
            }
            var regex = /^[a-zA-Z ]*$/;
            isValid = regex.test(Firsttext);
            if (isValid == false) {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Please Enter the Charcaters in First Name")
                ASPxPopupControl1.Show()
                return false;
            }
            if (Lasttext == "") {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Please Enter the Last Name")
                ASPxPopupControl1.Show()
                return false;
            }
            var regex = /^[a-zA-Z ]*$/;
            isValid = regex.test(Lasttext);
            if (isValid == false) {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Please Enter the Charcaters in Last Name")
                ASPxPopupControl1.Show()
                return false;
            }
            if (dateofbirth == null) {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Please Enter the Date of Birth")
                ASPxPopupControl1.Show()
                return false;
            }
            if (email == "") {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Please Enter the Email Address")
                ASPxPopupControl1.Show()
                return false;
            }
            var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            isValid = regex.test(email);
            if (isValid == false) {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Invalid Email Address")
                ASPxPopupControl1.Show()
                return false;
            }
            if (male == false && female == false) {
                ASPxPopupControl1.HeaderText = "Error";
                ASPxLabel8.SetText("Please Select Gender")
                ASPxPopupControl1.Show()
                return false;
            }
            if (confirm('Do you really want to add?')) {
                if (!ASPxClientEdit.ValidateGroup("addRow")) return;
                document.getElementById('<%=hdWhichButton.ClientID%>').value = "addRow";
                lp.Show();
               cab.PerformCallback();
            }
            else {
                e.processOnServer = false;
            }           
        };   
        function cancel(s, e) {
            dxPopUpError.Hide();
        }
        function TextBoxKeyDown(s, e) {
            if (!((e.htmlEvent.keyCode >= 48 && e.htmlEvent.keyCode <= 57)
            ))
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
        }
        function Charcatervalidation(s, e) {
            if (!((e.htmlEvent.keyCode >= 64 && e.htmlEvent.keyCode <= 123)
            ))
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
        }
        function confirmdelete(s, e) {
            if (confirm('Do you really want to delete?')) {
                e.processOnServer = true;
            }

            else {
                e.processOnServer = false;
            }
        }
        function alertOnUpload(s, e) {
            debugger                    
            if (e.isValid == true) {
                alert("File uploaded sucessfully")
            }
            else {
                alert("File already exist")
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SideHolder" runat="server">
    <table>
        <tr>
            <td>
                <dx:ASPxDockPanel runat="server" ID="ASPxDockPanel1" PanelUID="DateTime" HeaderText="Date & Time"
                    Height="95px" ClientInstanceName="dateTimePanel" Width="230px" OwnerZoneUID="zone1">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                            <widget:DateTime ID="xDTWid" runat="server" />
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxDockPanel>
            </td>
        </tr>
    </table>
    <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="229px" ZoneUID="zone1"
        PanelSpacing="3px" ClientInstanceName="splitter" Height="400px">
    </dx:ASPxDockZone>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cab"
        OnCallback="ASPxCallback1_Callback" SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>

                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="500px" CssClass="date_panel" HeaderText="Add Employee" Theme="DevEx">
                    <PanelCollection>
                        <dx:PanelContent>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Employee Number">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="EmployeeNumber" runat="server" Width="170px" ClientInstanceName="EmployeeNumber">
                                            <ClientSideEvents KeyPress="TextBoxKeyDown" />

                                        </dx:ASPxTextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="First Name">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="FirstName" runat="server" Width="170px" ClientInstanceName="FirstName">
                                            <ClientSideEvents KeyPress="Charcatervalidation" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Last Name">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="LastName" runat="server" Width="170px" ClientInstanceName="LastName">
                                            <ClientSideEvents KeyPress="Charcatervalidation" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Active">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="Active" runat="server" CheckState="Unchecked" ClientInstanceName="Active">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Date of Birth">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="DateofBirth" runat="server" ClientInstanceName="DateofBirth" DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd">
                                        </dx:ASPxDateEdit>

                                    </td>
                                </tr>
                                <tr>
                                    <td>

                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Email Address">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="EmailAddress" runat="server" Width="170px" ClientInstanceName="EmailAddress">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Gender">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>Male   
                                        <dx:ASPxRadioButton ID="Male" runat="server" GroupName="gender" ClientInstanceName="Male" Checked="false"></dx:ASPxRadioButton>
                                        Female  
                                        <dx:ASPxRadioButton ID="Female" runat="server" GroupName="gender" ClientInstanceName="Female" Checked="false"></dx:ASPxRadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="button">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Add">
                             <ClientSideEvents Click="addRow"></ClientSideEvents>

                         </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                    <dx:ASPxLabel ID="uploadfile" runat="server" Text="Upload File"></dx:ASPxLabel>
                                </td>
                                    <td>
                                        <dx:ASPxUploadControl ID="ASPxUploadControl1" runat="server" UploadMode="Auto" Width="280px" ShowUploadButton="true" ShowProgressPanel="true"
                                            ClientInstanceName="ASPxUploadControl1" ShowTextBox="true" OnFileUploadComplete="ASPxUploadControl1_FileUploadComplete">
                                            <ValidationSettings AllowedFileExtensions=".txt,.png"  MaxFileSize="2050000" ShowErrors="true" ></ValidationSettings>
                                            <ClientSideEvents FileUploadComplete="function(s,e){ alertOnUpload(s,e);}" /> 
                                                                                     
                                        </dx:ASPxUploadControl>
                                    </td>
                                    </tr>                             
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                &nbsp;
                          &nbsp;                                     
                          <br />                                     
                          <dx:ASPxGridView ID="ASPxGridView1" SettingsBehavior-ConfirmDelete="true" CssClass="date_panel" runat="server" ClientInstanceName="ASPxGridView1" AutoGenerateColumns="False"
                              ViewStateMode="Enabled" KeyFieldName="employee_number" Settings-ShowFilterRow="true">

                              <SettingsAdaptivity>
                                  <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                              </SettingsAdaptivity>

                              <Settings ShowFilterRow="True" />
                              <SettingsBehavior ConfirmDelete="True" />

                              <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>
                              <Columns>
                                  <dx:GridViewDataTextColumn FieldName="employee_number" Caption="EmployeeNumber" />
                                  <dx:GridViewDataTextColumn FieldName="first_name" Caption="FirstName" />
                                  <dx:GridViewDataTextColumn FieldName="last_name" Caption="LastName" />
                                  <dx:GridViewDataTextColumn FieldName="active" Caption="Active" />
                                  <dx:GridViewDataTextColumn FieldName="date_of_birth" Caption="DateofBirth" />
                                  <dx:GridViewDataTextColumn FieldName="email_address" Caption="EmailAddress" />
                                  <dx:GridViewDataTextColumn FieldName="gender" Caption="Gender" />
                                  <dx:GridViewCommandColumn  ShowEditButton="true" Caption="Action">
                                      <CustomButtons>
                                          <dx:GridViewCommandColumnCustomButton ID="delete" Text="Delete" >
                                          </dx:GridViewCommandColumnCustomButton>
                                      </CustomButtons>
                                  </dx:GridViewCommandColumn>

                              </Columns>
                              <ClientSideEvents CustomButtonClick="confirmdelete" />
                          </dx:ASPxGridView>
                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Confirm" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2">
                                    <dx:ASPxLabel ID="lblError" ClientInstanceName="lblError" runat="server" Text="The User file was updated successfully."
                                        Font-Size="16px">
                                    </dx:ASPxLabel>
                                    <table style="border: none">
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="btnOK" runat="server" AutoPostBack="False" Text="OK" Width="80px" ClientInstanceName="btnOK">
                                                       <%--<ClientSideEvents Click="addrowconfirm" />--%>
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel"
                                                    Text="Cancel" Width="80px">
                                                    <ClientSideEvents Click="cancel" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="error" Width="548px" CloseAction="None" ClientInstanceName="ASPxPopupControl1"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <div>
                                <div id="Div3">
                                    <dx:ASPxLabel ID="ASPxLabel8" ClientInstanceName="ASPxLabel8" runat="server"
                                        Font-Size="16px">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>                
                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>

