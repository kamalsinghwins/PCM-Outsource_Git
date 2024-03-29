﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="CardAssign.aspx.vb" Inherits="pcm.Website.CardAssign" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../js/General/application.js"></script>
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
            width: 150px;
        }

        .auto-style4 {
            width: 104px;
            height: 100%;
        }

        .main_view {
            margin: 20px 0px 0px 20px;
        }

            .main_view .dxtc-strip {
                height: auto !important;
            }

            .main_view table tr td span, .main_view a > .dx-vam {
                text-transform: uppercase;
            }

        .UpperCase {
            text-transform: uppercase;
        }

        .auto-style8 {
            margin-left: 40px;
        }

        .burea_btn {
            margin: 0 5px 10px 0;
        }

        .text-center {
            text-align: center;
        }

        .mb-10 {
            margin-bottom: 10px;
        }

        .mainContainer {
            padding: 10px;
        }
    </style>



    <script type="text/javascript">

        function onEnd(s, e) {
            lp.Hide();
        }
        function AutoIncrease(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "AutoIncrease";

            lp.Show();
            cab.PerformCallback();
        }


        function clear(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "clear";

            lp.Show();
            cab.PerformCallback();
        }

        function CheckIDClick(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "CheckID";

            lp.Show();
            cab.PerformCallback();
        }

        <%--function CheckID(s, e) {
            var ID = s.GetValue();
            if (ID && ID.length == 13) {
                e.processOnServer = false;
                e.processOnServer = false;

                document.getElementById('<%=hdWhichButton.ClientID%>').value = "CheckID";

                lp.Show();
                cab.PerformCallback();
            }
            else {
                return false;
            }
        }--%>

        function Accept(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Accept";

            lp.Show();
            cab.PerformCallback();
        }

        function cardInputChanged(s, e) {
            debugger;
            var card = s.GetValue();
            if (card && card.length == 21) {
                e.processOnServer = false;

                document.getElementById('<%=hdWhichButton.ClientID%>').value = "CardChanged";

                lp.Show();
                cab.PerformCallback();
            }
        }

        function IsNumeric(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            return ret;
        }
        function noAlphabets(e) {
            var regex = new RegExp("[a-zA-Z0-9@ ]");
            var key = e.keyCode || e.which;
            key = String.fromCharCode(key);
            if (!regex.test(key)) {
                e.returnValue = false;
                if (e.preventDefault) {
                    e.preventDefault();
                }
            }
        }
        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]/;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
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
        OnCallback="ASPxCallback1_Callback"
        SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>

        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>

                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssClass="date_panel" HeaderText="Card Assign" Width="90%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <div class="left-side-tables" style="width: 100%; float: left;">
                                    <table>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblIdNumber" runat="server" Text="ID Number   ">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtIDNumber" runat="server" Width="170px" MaxLength="13">
                                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxButton ID="cmdCheck" runat="server" Text="Check">
                                                    <ClientSideEvents Click="CheckIDClick"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblStatus" runat="server" Text="Status">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <%-- <dx:ASPxTextBox ID="txtFrom" onKeyPress="javascript:return isNumber(event)" runat="server" Width="170px">
                                        </dx:ASPxTextBox>--%>
                                                <dx:ASPxTextBox ID="txtStatus" runat="server" Width="170px" MaxLength="30" ReadOnly="True"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblCreditLimit" runat="server" Text="Credit Limit">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <%-- <dx:ASPxTextBox ID="txtFrom" onKeyPress="javascript:return isNumber(event)" runat="server" Width="170px">
                                        </dx:ASPxTextBox>--%>
                                                <dx:ASPxTextBox ID="txtCreditLimit" runat="server" Width="170px" MaxLength="30" ReadOnly="True"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblFirstName" runat="server" Text="First Name">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtFirstName" runat="server" Width="170px" MaxLength="30"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblSurname" runat="server" Text="Surname">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtSurname" runat="server" Width="170px" MaxLength="30"></dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblCellphone" runat="server" Text="Cellphone">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtCellphone" runat="server" Width="170px" MaxLength="12" onkeypress="return IsNumeric(event);" ondrop="return false;" onpaste="return false;">
                                                    <MaskSettings ErrorText="Please input missing digits" Mask="999-999-9999" />
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblLostCard" runat="server" Text="Lost Card">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxCheckBox ID="chkLostCard" runat="server"></dx:ASPxCheckBox>
                                               <%-- <dx:ASPxLabel ID="lbllostCardCharge" runat="server" Text="" Visible="False"></dx:ASPxLabel>--%>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblAutoIncrease" runat="server" Text="Auto Increase">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxCheckBox ID="chkAutoIncrease" runat="server">
                                                    <%--<ClientSideEvents CheckedChanged="AutoIncrease"/>--%>
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblCardNumber" runat="server" Text="Card Number">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtCardNumber" runat="server" Width="170px" MaxLength="50">
                                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                    <%--<ClientSideEvents ValueChanged="cardInputChanged" />--%>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtEmployeeNumber" runat="server" Width="170px" MaxLength="10" onkeypress="return noAlphabets(event)" ondrop="return false;" onpaste="return false;">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblBranch" runat="server" Text="Branch">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxComboBox ID="cboBranch" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="lblPreferredLanguage" runat="server" Text="Preferred Language">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxComboBox ID="cboLanguage" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="lblLostCardCharge" runat="server" Text="Lost Card Charge" Visible="false">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtLostCardCharge" runat="server" MaxLength="50" Width="170px" ReadOnly="true" Visible="false">
                                                    
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style8">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td></td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8" colspan="2">
                                                <asp:Label ID="lblActivate" runat="server" Text="This account cannot be Self-Activated. Please call the Call-Centre for Activation." ForeColor="Red" Visible="False"></asp:Label>

                                            </td>
                                        </tr>
                                    </table>
                                    <br />

                                    <table style="width: 100%">
                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="auto-style8" colspan="2">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <dx:ASPxButton ID="cmdClear" runat="server" Text="Clear">
                                                    <ClientSideEvents Click="clear"></ClientSideEvents>
                                                </dx:ASPxButton>
                                                <dx:ASPxButton ID="cmdAccept" runat="server" Text="Accept">
                                                    <ClientSideEvents Click="Accept"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style8" colspan="2">
                                                <dx:ASPxLabel ID="lblOverRide" runat="server" Text="OVERRIDE"></dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td class="auto-style8" colspan="2">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                    <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                                        HeaderText="" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                                        DisappearAfter="1000" PopupAnimationType="Fade">
                                        <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                                        <ContentCollection>
                                            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                                                <div>
                                                    <div id="Div2" class="text-center">
                                                        <dx:ASPxLabel ID="lblError" runat="server"
                                                            Font-Size="16px">
                                                        </dx:ASPxLabel>
                                                    </div>
                                                </div>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                        <ClientSideEvents CloseButtonClick="fadeOut" />
                                    </dx:ASPxPopupControl>
                                </div>

                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                </div>

                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

</asp:Content>


