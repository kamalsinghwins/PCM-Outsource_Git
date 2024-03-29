﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="investigations.aspx.vb" Inherits="pcm.Website.investigations" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/Collections/contactinvestigation.js"></script>
    <script src="../js/General/jquery-2.0.3.min.js"></script>
    <script src="../js/General/application.js"></script>
    <style type="text/css">
        .style3 {
        }

        .style4 {
            width: 104px;
        }

        .style6 {
            width: 422px;
        }

        .auto-style1 {
            width: 104px;
            height: 23px;
        }

        .auto-style2 {
            height: 23px;
        }

        .auto-style3 {
            width: 104px;
        }

        .auto-style4 {
            width: 104px;
            height: 100%;
        }

             .auto-style6 {
            width: 448px;
        }
        
    </style>
    <script>
        function onClick(s, e) {
            e.processOnServer = false;
            //do validation here
            if (!ASPxClientEdit.ValidateGroup("save")) return;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Save";

             lp.Show();
             cab.PerformCallback();

        }

        function onBankingDetails(s, e) {
            e.processOnServer = false;
            //do validation here


            //var cn = ASPxTextBox.GetValue();
            //if (cn == null) {
            //    alert('Please select an Account before trying to send a Banking Details SMS!');
            //    return false;
            //}

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "BankingDetails";

            lp.Show();
            cab.PerformCallback();

        }

        function onClickGetDebtor(s, e) {
            e.processOnServer = false;
            //do validation here
           
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "GetDebtor";

            lp.Show();
            cab.PerformCallback();

        }

         function onClickNext(s, e) {
             e.processOnServer = false;
             //do validation here
             if (!ASPxClientEdit.ValidateGroup("save")) return;

             document.getElementById('<%=hdWhichButton.ClientID%>').value = "Next";

            lp.Show();
            cab.PerformCallback();

         }

        function onClickNewBureau(s, e) {
            e.processOnServer = false;
            //do validation here
            //if (!ASPxClientEdit.ValidateGroup("save")) return;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "NewBureau";

            lp.Show();
            cab.PerformCallback();

        }

        function onEnd(s, e) {
            lp.Hide();

        }

        function NumericOnly(s, e) {
            if (e.htmlEvent.keyCode != 46 && e.htmlEvent.keyCode > 31
            && (e.htmlEvent.keyCode < 48 || e.htmlEvent.keyCode > 57))
                return _aspxPreventEvent(e.htmlEvent);

            return true;
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
                <dx:ASPxNavBar ID="ASPxNavBar2" runat="server">
                    <Groups>
                        <dx:NavBarGroup Name="GetNewDebtor" Text="Get New Debtor">
                            <Items>
                               <%-- <dx:NavBarItem Name="30days" Text="30 Days">
                                </dx:NavBarItem>--%>
                                <dx:NavBarItem Name="GetNewDebtor" Text="Get Debtor">
                                </dx:NavBarItem>
                              
                            </Items>
                        </dx:NavBarGroup>
                    </Groups>
                </dx:ASPxNavBar>
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
        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <table class="main_table">
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Account Number">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtAccountNumber" runat="server" Width="170px"
                                EnableClientSideAPI="True" ClientInstanceName="ASPxTextBox" ValueType="System.String"
                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                ValidationSettings-ValidationGroup="save">
                                <ValidationSettings SetFocusOnError="True" ErrorText="You must have an opened Account in order to Save">
                                    <RequiredField IsRequired="True" ErrorText="You must have an opened Account in order to Save"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxButton ID="cmdGetDebtor" runat="server" Text="Get Debtor" AutoPostBack="false">
                                <ClientSideEvents Click="onClickGetDebtor"></ClientSideEvents>
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblNextContact" runat="server" Text="Next Contact Details">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="First Name">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtFirstName" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Last Name">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtLastName" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Credit Limit">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtCreditLimit" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;
                        </td>
                       <td>
                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Consumer Rating">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                           
                        </td>
                        <td>
                             <dx:ASPxTextBox ID="txtConsumerRating" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Last Sale Date">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtLastSaleDate" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Last Sale Amount">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtLastSaleAmount" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Last Payment Date">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtLastPaymentDate" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Last Payment Amount">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtLastPaymentAmount" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Balance">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtBalance" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Overdue">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtOverdue" runat="server" Width="170px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel32" runat="server" Text="Status">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtStatus" runat="server" ReadOnly="True" Width="170px">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td> <asp:Button ID="Button1" runat="server" Text="Button" Visible="False" />
                            <asp:Button ID="Button2" runat="server" Text="Button" Visible="False" /></td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Send Promos">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="style3">
                            <dx:ASPxCheckBox ID="chkSendPromos" runat="server" CheckState="Unchecked">
                            </dx:ASPxCheckBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style3">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="style3">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Contact Number">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtContactNumber" runat="server" Width="170px" ValidationSettings-ValidationGroup="save" ClientInstanceName="contactnumber">
                                <MaskSettings Mask="999-999-9999" />
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="Text" ErrorText="You must enter the Primary Number in full.">
                                </ValidationSettings>
                                <ClientSideEvents Validation="OnContactNumberValidation"></ClientSideEvents>
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style4">
                            <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Home Number 1">
                            </dx:ASPxLabel>
                        </td>
                        <td class="auto-style2"></td>
                        <td class="auto-style2"></td>
                        <td class="auto-style2">
                            <dx:ASPxTextBox ID="txtHomeNumber1" runat="server" ValidationGroup="save" Width="170px">
                                <MaskSettings ErrorText="Please input missing digits" Mask="999-999-9999" />
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip">
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td></td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Home Number 2">
                            </dx:ASPxLabel>
                        </td>
                        <td></td>
                        <td>
                            <dx:ASPxTextBox ID="txtHomeNumber2" runat="server" ValidationGroup="save" Width="170px">
                                <MaskSettings ErrorText="Please input missing digits" Mask="999-999-9999" />
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip">
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Next of Kin">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtNextOfKin" runat="server" ValidationGroup="save" Width="170px">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Next of Kin Number">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxTextBox ID="txtNextOfKinNumber" runat="server" ValidationGroup="save" Width="170px">
                                <MaskSettings ErrorText="Please input missing digits" Mask="999-999-9999" />
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip">
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Work Number">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtWorkNumber" runat="server" ValidationGroup="save" Width="170px">
                                <MaskSettings ErrorText="Please input missing digits" Mask="999-999-9999" />
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip">
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Alt Number">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxTextBox ID="txtAltNumber" runat="server" ValidationGroup="save" Width="170px">
                                <MaskSettings ErrorText="Please input missing digits" Mask="999-999-9999" />
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip">
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="Spouse Number">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="style3">
                            <dx:ASPxTextBox ID="txtSpouseContactNumber" runat="server" ValidationGroup="save" Width="170px">
                                <MaskSettings ErrorText="Please input missing digits" Mask="999-999-9999" />
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Preferred Language">
                            </dx:ASPxLabel></td>
                        <td>&nbsp;</td>
                        <td><dx:ASPxComboBox ID="cboPreferredLanguage" runat="server" EnableClientSideAPI="True" ClientInstanceName="cbl"
                                ValueType="System.String" ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                ValidationSettings-ValidationGroup="save">
                                <ValidationSettings SetFocusOnError="True" ErrorText="Please select a Preferred Language">
                                    <RequiredField IsRequired="True" ErrorText="Please select a Preferred Language" />
                                    <RequiredField IsRequired="True" ErrorText="Please select a Preferred Language"></RequiredField>
                                </ValidationSettings>
                                <%--<ClientSideEvents Init="function (s,e) {s.GetText;} " />--%>
                            </dx:ASPxComboBox></td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="PTP Date">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="style3"> <dx:ASPxDateEdit ID="txtPTPDate" runat="server" ClientInstanceName="pd" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                <CalendarProperties ShowClearButton="False">
                                </CalendarProperties>
                                <ClientSideEvents Validation="OnPTPDateValidation" />
                                <ValidationSettings Display="Dynamic" ErrorText="Please select a valid PTP Date" SetFocusOnError="True" ValidationGroup="save">
                                </ValidationSettings>
                            </dx:ASPxDateEdit></td>
                        <td>&nbsp;</td>
                        <td> <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="PTP Amount">
                            </dx:ASPxLabel></td>
                        <td>&nbsp;</td>
                        <td><dx:ASPxTextBox ID="txtPTPAmount" runat="server" ClientInstanceName="tp" Mask="0.00" MaskType="Numeric" Width="170px">
                                <ClientSideEvents KeyPress="NumericOnly" Validation="OnPTPValidation" />
                                <ValidationSettings Display="Dynamic" ErrorText="Please enter a PTP Amount" SetFocusOnError="True" ValidationGroup="save">
                                </ValidationSettings>

                                
                            </dx:ASPxTextBox></td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Call Result">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td class="style3">
                            <dx:ASPxComboBox ID="cboResult" runat="server" EnableClientSideAPI="True" ClientInstanceName="cb"
                                ValueType="System.String" ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                ValidationSettings-ValidationGroup="save">
                                <ValidationSettings SetFocusOnError="True" ErrorText="Please select a Call Result">
                                    <RequiredField IsRequired="True" ErrorText="Please select a Call Result"></RequiredField>
                                </ValidationSettings>
                                <%--<ClientSideEvents Init="function (s,e) {s.GetText;} " />--%>
                            </dx:ASPxComboBox>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <table class="notes_table">
                    <tr>
                        <td class="style4">
                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Notes">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <dx:ASPxMemo ID="txtNotes" runat="server" Height="71px" Width="520px" ClientInstanceName="notes"
                                ValidationSettings-Display="Dynamic" ValidationSettings-ValidateOnLeave="true"
                                ValidationSettings-ValidationGroup="save" ValidationSettings-ErrorText="ttt">
                                <ClientSideEvents KeyDown="RecalculateCharsRemaining" KeyUp="RecalculateCharsRemaining"
                                    GotFocus="EnableMaxLengthMemoTimer" LostFocus="DisableMaxLengthMemoTimer"
                                    Init="function(s, e) { InitMemoMaxLength(s, 400); RecalculateCharsRemaining(s); }"></ClientSideEvents>
                                <ValidationSettings SetFocusOnError="True" ErrorText="You must fill in a note if you are marking an account as 'Under Investigation'">
                                </ValidationSettings>
                            </dx:ASPxMemo>
                            <span class="chrm">
                                <dx:ASPxLabel ID="txtNotes_cr" runat="server" EnableClientSideAPI="True" />
                            </span>
                            <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" rend
                                ValidationGroup="save">
                            </dx:ASPxValidationSummary>
                        </td>
                    </tr>
                </table>
                    <table>
                    <tr>
                        <td class="auto-style6">
                            <dx:ASPxButton ID="cmdNewBureau" CssClass="new_bureau" runat="server" Text="Get New Bureau Data"
                                Width="130px" AutoPostBack="false" Enabled="False">
                                <ClientSideEvents Click="onClickNewBureau"></ClientSideEvents>
                            </dx:ASPxButton>
                            &nbsp;
                              <dx:ASPxButton ID="cmdBankingDetails" runat="server" AutoPostBack="False" Enabled="False" CssClass="new_bureau" 
                                Text="Send Rage Banking Details" Width="130px">
                                <ClientSideEvents Click="onBankingDetails" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="cmdSave" CssClass="float_right_menu" runat="server" Text="Save"
                                Width="130px" AutoPostBack="false">
                                <ClientSideEvents Click="onClick"></ClientSideEvents>
                            </dx:ASPxButton>
                            &nbsp;
                        </td>
                        <td class="style5">
                        </td>
                        <td>
                          
                            <dx:ASPxButton ID="cmdSaveAndNext" CssClass="float_right_menu" runat="server" Text="Save and Next"
                                Width="130px" AutoPostBack="false">
                                <ClientSideEvents Click="onClickNext"></ClientSideEvents>
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
               <%-- <table>
                    <tr>
                        <td class="style6"><dx:ASPxButton ID="cmdBankingDetails" runat="server" AutoPostBack="False" Enabled="False" CssClass="float_right_menu" 
                                Text="Send Rage Banking Details" Width="130px">
                                <ClientSideEvents Click="onBankingDetails" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                        </td>
                        <td style="width:130px"></td>
                        <td> <dx:ASPxButton ID="cmdSave" CssClass="float_right_menu" runat="server" Text="Save"
                                Width="130px" AutoPostBack="false">
                                <ClientSideEvents Click="onClick"></ClientSideEvents>
                            </dx:ASPxButton>
                         
                        </td>
                    </tr>
                </table>--%>
                <table class="main_table">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Age Analysis:">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="grdAgeAnalysis" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                OnDataBinding="gridAgeAnalysis_DataBinding">
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Total" FieldName="aaTotal" VisibleIndex="0" Width="100px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Current" FieldName="aaCurrent" VisibleIndex="1"
                                        Width="100px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="30 Days" FieldName="aa30Days" VisibleIndex="2"
                                        Width="100px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="60 Days" FieldName="aa60Days" VisibleIndex="3"
                                        Width="100px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="90 Days" FieldName="aa90Days" VisibleIndex="4"
                                        Width="100px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="120 Days" FieldName="aa120Days" VisibleIndex="5"
                                        Width="100px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="150 Days" FieldName="aa150Days" VisibleIndex="6"
                                        Width="100px">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsPager PageSize="20">
                                </SettingsPager>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxNavBar ID="ASPxNavBar1" runat="server" Width="100%">
                                <Groups>
                                    <dx:NavBarGroup Expanded="False" Text="Account Change History" Name="ChangeHistory">
                                        <Items>
                                            <dx:NavBarItem>
                                                <Template>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="grdAccountHistory" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                                    OnDataBinding="gridAccountHistory_DataBinding">
                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="Date" FieldName="ChangeDate" VisibleIndex="0"
                                                                            Width="120px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Time" VisibleIndex="1" FieldName="ChangeTime"
                                                                            Width="120px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Description" FieldName="ChangeDescription" VisibleIndex="2"
                                                                            Width="360px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Old Value" FieldName="OldValue" VisibleIndex="2"
                                                                            Width="140px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="New Value" FieldName="NewValue" VisibleIndex="3"
                                                                            Width="140px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Username" FieldName="Username" VisibleIndex="3"
                                                                            Width="160px">
                                                                        </dx:GridViewDataTextColumn>

                                                                    </Columns>
                                                                    <SettingsPager PageSize="20">
                                                                    </SettingsPager>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </Template>
                                            </dx:NavBarItem>

                                        </Items>
                                    </dx:NavBarGroup>
                                    <dx:NavBarGroup Expanded="False" Text="Alternative Numbers" Name="AlternativeNumbers">
                                        <Items>
                                            <dx:NavBarItem>
                                                <Template>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="grdAlternativeNumbers" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                                    OnDataBinding="gridAlternativeNumbers_DataBinding">
                                                                    <Columns>
                                                                       
                                                                        <dx:GridViewDataTextColumn Caption="Number" VisibleIndex="0" FieldName="NumberNumber"
                                                                            Width="180px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Update Date" FieldName="NumberUpdateDate" VisibleIndex="1"
                                                                           Width="360px">
                                                                        </dx:GridViewDataTextColumn>
                                                                       <dx:GridViewDataTextColumn Caption="Date of Record Insert" FieldName="DateRecordInserted" VisibleIndex="2"
                                                                           Width="360px">
                                                                        </dx:GridViewDataTextColumn>
                                                                      
                                                                    </Columns>
                                                                    <SettingsPager PageSize="20">
                                                                    </SettingsPager>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </Template>
                                            </dx:NavBarItem>
                                           
                                        </Items>
                                    </dx:NavBarGroup>
                                    <dx:NavBarGroup Expanded="False" Text="Contact History" Name="ContactHistory">
                                        <Items>
                                            <dx:NavBarItem>
                                                <Template>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="grdHistory" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                                    OnDataBinding="gridHistory_DataBinding">
                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="Date / Time" FieldName="TimeStampOfAction" VisibleIndex="0"
                                                                            Width="160px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="User" VisibleIndex="1" Width="150px" FieldName="ActionUser">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Type" FieldName="TypeOfContact" VisibleIndex="2"
                                                                            Width="160px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Result" FieldName="ResultOfAction" VisibleIndex="2"
                                                                            Width="160px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="PTP Amount" FieldName="PTPAmount" VisibleIndex="3"
                                                                            Width="70px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="PTP Date" FieldName="PTPDate" VisibleIndex="3"
                                                                            Width="100px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Notes" FieldName="ActionNotes" VisibleIndex="4"
                                                                            Width="200px">
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <SettingsPager PageSize="20">
                                                                    </SettingsPager>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </Template>
                                            </dx:NavBarItem>
                                        </Items>
                                    </dx:NavBarGroup>
                                    <dx:NavBarGroup Expanded="False" Text="Transactions" Name="Transactions">
                                        <Items>
                                            <dx:NavBarItem>
                                                <Template>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="grdTransactions" runat="server" AutoGenerateColumns="False"
                                                                    EnableTheming="True" OnDataBinding="gridTransactions_DataBinding">
                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="Date" FieldName="tDate" VisibleIndex="0" Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Time" FieldName="tTime" VisibleIndex="1" Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="User" VisibleIndex="2" Width="150px" FieldName="tUser">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Type" FieldName="tType" VisibleIndex="3" Width="70px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Reference" FieldName="tReference" VisibleIndex="4"
                                                                            Width="110px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Amount" FieldName="tAmount" VisibleIndex="5"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Period" FieldName="tPeriod" VisibleIndex="6"
                                                                            Width="40px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Balance" FieldName="tRunningBalance" VisibleIndex="7"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <SettingsPager PageSize="20">
                                                                    </SettingsPager>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </Template>
                                            </dx:NavBarItem>
                                        </Items>
                                    </dx:NavBarGroup>
                                    <dx:NavBarGroup Text="Payment Plans" Expanded="False" Name="PaymentPlans">
                                        <Items>
                                            <dx:NavBarItem>
                                                <Template>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="grdPaymentPlans" runat="server" AutoGenerateColumns="False"
                                                                    EnableTheming="True" OnDataBinding="gridPayments_DataBinding">
                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="Date" FieldName="ppDate" VisibleIndex="0" Width="90px"
                                                                            UnboundType="String">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Time" FieldName="ppTime" VisibleIndex="1" Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Total" FieldName="ppTotal" VisibleIndex="2" Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Period 1" FieldName="ppPeriod1" VisibleIndex="3"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Amount 1" FieldName="ppAmount1" VisibleIndex="4"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Period 2" FieldName="ppPeriod2" VisibleIndex="5"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Amount 2" VisibleIndex="6" Width="90px" FieldName="ppAmount2">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Period 3" FieldName="ppPeriod3" VisibleIndex="7"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Amount 3" FieldName="ppAmount3" VisibleIndex="8"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Period 4" FieldName="ppPeriod4" VisibleIndex="9"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Amount 4" FieldName="ppAmount4" VisibleIndex="10"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Period 5" FieldName="ppPeriod5" VisibleIndex="11"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Amount 5" FieldName="ppAmount5" VisibleIndex="12"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Period 6" FieldName="ppPeriod6" VisibleIndex="13"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Amount 6" FieldName="ppAmount6" VisibleIndex="14"
                                                                            Width="90px">
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </Template>
                                            </dx:NavBarItem>
                                        </Items>
                                    </dx:NavBarGroup>
                                    <dx:NavBarGroup Text="Closing Balances" Expanded="False" Name="ClosingBalances">
                                        <Items>
                                            <dx:NavBarItem>
                                                <Template>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="grdClosingBalances" runat="server" AutoGenerateColumns="False"
                                                                    EnableTheming="True" OnDataBinding="gridClosingBalances_DataBinding">
                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="Period" FieldName="cbPeriod" VisibleIndex="0"
                                                                            Width="100px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Total" FieldName="cbTotal" VisibleIndex="1" Width="100px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="30 Days" FieldName="cb30Days" VisibleIndex="2"
                                                                            Width="100px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="60 Days" FieldName="cb60Days" VisibleIndex="3"
                                                                            Width="100px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="90 Days" FieldName="cb90Days" VisibleIndex="4"
                                                                            Width="100px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="120 Days" FieldName="cb120Days" VisibleIndex="5"
                                                                            Width="100px">
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="150 Days" FieldName="cb150Days" VisibleIndex="6"
                                                                            Width="100px">
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </Template>
                                            </dx:NavBarItem>
                                        </Items>
                                    </dx:NavBarGroup>
                                </Groups>
                            </dx:ASPxNavBar>

                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="dxPopUp" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Info" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpPanel"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <div>
                                <div id="offer">
                                    <dx:ASPxLabel ID="lblStatus" runat="server" Text="Account Updated" Font-Size="16px">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Error" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2">
                                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="There was an error updating this account. Please contact support."
                                        Font-Size="16px">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ShowCloseButton="True"
                    Style="margin-right: 328px" HeaderText="Next Contact Details" Width="548px" CloseAction="None"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade" PopupElementID="lblNextContact">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                            <div>
                                <div id="Div1">
                                    <table>
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Current Contact Level: ">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="lblContactLevel" runat="server" Text="0">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Next Contact Date: ">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="lblNextDate" runat="server" Text="">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>
                <asp:HiddenField ID="hdContactNumber" runat="server" />
                <asp:HiddenField ID="hdHomeNumber1" runat="server" />
                <asp:HiddenField ID="hdHomeNumber2" runat="server" />
                <asp:HiddenField ID="hdAltNumber" runat="server" />
                <asp:HiddenField ID="hdNextOfKin" runat="server" />
                <asp:HiddenField ID="hdNextOfKinNumber" runat="server" />
                <asp:HiddenField ID="hdSpouseContactNumber" runat="server" />
                <asp:HiddenField ID="hdWorkNumber" runat="server" />
                <asp:HiddenField ID="hdSendPromos" runat="server" />
                <asp:HiddenField ID="hdTimeDataServed" runat="server" />
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <asp:HiddenField ID="hdStatus" runat="server" />
                <asp:HiddenField ID="hdPreferredLanguage" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <asp:HiddenField ID="hdUsername" runat="server" />
</asp:Content>
