<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="BadDebt.aspx.vb" Inherits="pcm.Website.BadDebt" %>

<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>
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

        .auto-style3 {
            width: 150px;
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

        .check-with-input {
            float: left;
            margin-right: 10px;
        }

        .input-check-with {
            float: left;
            width: 140px;
        }

        .auto-style11 {
            width: 150px;
            height: 18px;
        }

        .auto-style12 {
            height: 18px;
        }

        .auto-style13 {
            margin-left: 40px;
            height: 18px;
        }
    </style>

    <script type="text/javascript">

        function SubmitForm(s, e) {
            debugger;
            e.processOnServer = false;
            //do validation here
            if (!ASPxClientEdit.ValidateGroup("ErrorGroup")) return;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "CreateSurvey";

            lp.Show();
            cab.PerformCallback();
        }


        function onEnd(s, e) {
            lp.Hide();
        }


        function Ok(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Ok";

            lp.Show();
            cab.PerformCallback();
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
                <div class="mainContainer">
                    <div class="left-side-tables" style="width: 100%; float: left;">
                        <fieldset>
                            <legend>Period</legend>
                            <table>
                                <tr>
                                    <td class="auto-style3">

                                        <dx:ASPxLabel ID="Label1_0" runat="server" Text="Select Period">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2">
                                        <dx:ASPxComboBox ID="cboPeriod" TextField="periods" DropDownStyle="DropDownList" runat="server" Width="140px" ValueType="System.String"></dx:ASPxComboBox>

                                    </td>
                                </tr>


                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2"></td>
                                    <td class="auto-style8"></td>
                                </tr>
                                <tr>
                                    <td class="auto-style3"></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td class="auto-style8" colspan="2" style="float: right">
                                        <dx:ASPxButton ID="cmdOk" runat="server" Text="RUN">
                                            <ClientSideEvents Click="Ok"></ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                    <td class="auto-style8"></td>
                                </tr>
                            </table>
                        </fieldset>
                        <table style="width: 100%">
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                    
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2">
                                    <dx:ASPxButton ID="cmdExportCSVBadDebt" runat="server" Text="Export to CSV">
                                    </dx:ASPxButton>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style3">
                                    <dx:ASPxLabel ID="Label1_1" runat="server" Text="Bad Debt">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;
                                 
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="color: #666666; font-family: Tahoma; font-size: 14px;" colspan="5">
                                    <dx:ASPxGridView ID="grdBadDebt" runat="server" AutoGenerateColumns="False" OnDataBinding="grdBadDebt_DataBinding"
                                        Width="100%">

                                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                        <%-- <ClientSideEvents RowDblClick="FillDebtorsDetails"></ClientSideEvents>--%>

                                        <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>

                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="account_number" Caption="Account Number" VisibleIndex="1" />
                                            <dx:GridViewDataTextColumn FieldName="current_period" Caption="Internal Period" VisibleIndex="2" />
                                            <dx:GridViewDataTextColumn FieldName="current_balance" Caption="Current Balance" VisibleIndex="3" />
                                            <dx:GridViewDataTextColumn FieldName="p30" Caption="p30" VisibleIndex="4" />
                                            <dx:GridViewDataTextColumn FieldName="p60" Caption="p60" VisibleIndex="5" />
                                            <dx:GridViewDataTextColumn FieldName="p90" Caption="p90" VisibleIndex="6" />
                                            <dx:GridViewDataTextColumn FieldName="p120" Caption="p120" VisibleIndex="7" />
                                            <dx:GridViewDataTextColumn FieldName="p150" Caption="p150" VisibleIndex="8" />
                                            <dx:GridViewDataTextColumn FieldName="total" Caption="Total" VisibleIndex="9" />
                                            <dx:GridViewDataTextColumn FieldName="purchase_amount" Caption="Purchase Amount" VisibleIndex="10" />
                                            <dx:GridViewDataTextColumn FieldName="interest_amount" Caption="Interest Amount" VisibleIndex="11" />

                                        </Columns>
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="current_balance" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p30" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p60" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p90" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p120" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p150" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="total" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="purchase_amount" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="interest_amount" SummaryType="Sum" />
                                        </TotalSummary>
                                        <SettingsAdaptivity>
                                            <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                                        </SettingsAdaptivity>

                                        <SettingsPager PageSize="50" />
                                        <Settings ShowFooter="True" />
                                    </dx:ASPxGridView>
                                </td>
                            </tr>

                           <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                    
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2">
                                    <dx:ASPxButton ID="cmdExportCSVBadDebtRecovered" runat="server" Text="Export to CSV">
                                    </dx:ASPxButton>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style3">
                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Bad Debt Recovered">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;
                                 
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="color: #666666; font-family: Tahoma; font-size: 14px;" colspan="5">
                                    <dx:ASPxGridView ID="grdBadDebtRecovered" runat="server" AutoGenerateColumns="False" OnDataBinding="grdBadDebtRecovered_DataBinding"
                                        Width="100%">

                                        <%-- <ClientSideEvents RowDblClick="FillDebtorsDetails"></ClientSideEvents>--%>

                                        <SettingsAdaptivity>
                                            <AdaptiveDetailLayoutProperties ColCount="1">
                                            </AdaptiveDetailLayoutProperties>
                                        </SettingsAdaptivity>
                                        <SettingsPager PageSize="50" />
                                        <Settings ShowFooter="True" />

                                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                        <EditFormLayoutProperties ColCount="1">
                                        </EditFormLayoutProperties>

                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="account_number" Caption="Account Number" VisibleIndex="1" />
                                            <dx:GridViewDataTextColumn FieldName="current_period" Caption="Internal Period" VisibleIndex="2" />
                                            <dx:GridViewDataTextColumn FieldName="current_balance" Caption="Current Balance" VisibleIndex="3" />
                                            <dx:GridViewDataTextColumn FieldName="p30" Caption="p30" VisibleIndex="4" />
                                            <dx:GridViewDataTextColumn FieldName="p60" Caption="p60" VisibleIndex="5" />
                                            <dx:GridViewDataTextColumn FieldName="p90" Caption="p90" VisibleIndex="6" />
                                            <dx:GridViewDataTextColumn FieldName="p120" Caption="p120" VisibleIndex="7" />
                                            <dx:GridViewDataTextColumn FieldName="p150" Caption="p150" VisibleIndex="8" />
                                            <dx:GridViewDataTextColumn FieldName="total" Caption="Total" VisibleIndex="9" />
                                            <dx:GridViewDataTextColumn FieldName="purchase_amount" Caption="Purchase Amount" VisibleIndex="10" />
                                            <dx:GridViewDataTextColumn FieldName="interest_amount" Caption="Interest Amount" VisibleIndex="11" />

                                        </Columns>
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="current_balance" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p30" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p60" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p90" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p120" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="p150" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="total" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="purchase_amount" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="interest_amount" SummaryType="Sum" />
                                        </TotalSummary>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td>&nbsp;
                                    
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2">
                                    <dx:ASPxButton ID="cmdExportCSVReductionIn150" runat="server" Text="Export to CSV">
                                    </dx:ASPxButton>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style3">
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Reduction In 150">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;
                                 
                                </td>
                                <td></td>
                                <td align="right" class="auto-style8" colspan="2"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="color: #666666; font-family: Tahoma; font-size: 14px;" colspan="5">
                                    <dx:ASPxGridView ID="grdReduction" runat="server" AutoGenerateColumns="False" OnDataBinding="grdReduction_DataBinding"
                                        Width="100%">

                                        <%-- <ClientSideEvents RowDblClick="FillDebtorsDetails"></ClientSideEvents>--%>

                                        <SettingsAdaptivity>
                                            <AdaptiveDetailLayoutProperties ColCount="1">
                                            </AdaptiveDetailLayoutProperties>
                                        </SettingsAdaptivity>
                                        <SettingsPager PageSize="50" />
                                        <Settings ShowFooter="True" />

                                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />
                                        <EditFormLayoutProperties ColCount="1">
                                        </EditFormLayoutProperties>

                                        <Columns>

                                            <dx:GridViewDataTextColumn FieldName="account_number" Caption="Account Number" VisibleIndex="1" />
                                            <dx:GridViewDataTextColumn FieldName="current_period" Caption="Internal Period" VisibleIndex="2" />
                                            <dx:GridViewDataTextColumn FieldName="prev_balance" Caption="Previous Balance" VisibleIndex="3" />
                                            <dx:GridViewDataTextColumn FieldName="new_balance" Caption="New Balance" VisibleIndex="4" />
                                            <dx:GridViewDataTextColumn FieldName="paid" Caption="Paid" VisibleIndex="5" />
                                            
                                            <dx:GridViewDataTextColumn FieldName="prev_purchase_amount" Caption="Prev Purchases" VisibleIndex="6" />
                                            <dx:GridViewDataTextColumn FieldName="purchase_amount" Caption="New Purchases" VisibleIndex="7" />
                                            <dx:GridViewDataTextColumn FieldName="paid_purchase" Caption="Paid Purchase" VisibleIndex="8" />
                                            
                                            <dx:GridViewDataTextColumn FieldName="prev_interest_amount" Caption="Prev Interest" VisibleIndex="9" />
                                            <dx:GridViewDataTextColumn FieldName="interest_amount" Caption="New Interest" VisibleIndex="10" />
                                            <dx:GridViewDataTextColumn FieldName="paid_interest" Caption="Paid Interest" VisibleIndex="11" />


                                        </Columns>
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="prev_balance" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="new_balance" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="paid" SummaryType="Sum" />
                                           
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="prev_purchase_amount" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="purchase_amount" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="paid_purchase" SummaryType="Sum" />
                                            
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="prev_interest_amount" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="interest_amount" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="paid_interest" SummaryType="Sum" />
                                        </TotalSummary>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <dx:ASPxGridView ID="grdHidden" runat="server" Visible="false">
<SettingsAdaptivity>
<AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
</SettingsAdaptivity>

<EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="1" />
                                    <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="2" />
                                    <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="3" />
                                    <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="4" />
                                    <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="5" />
                                    <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="6" />
                                    <dx:GridViewDataTextColumn FieldName="" Caption="" VisibleIndex="7" />

                                </Columns>
                            </dx:ASPxGridView>
                        </div>
                        <dx:ASPxGridViewExporter ID="GridExporter1" runat="server" GridViewID="grdBadDebt" />
                        <dx:ASPxGridViewExporter ID="GridExporter2" runat="server" GridViewID="grdBadDebtRecovered" />
                        <dx:ASPxGridViewExporter ID="GridExporter3" runat="server" GridViewID="grdReduction" />
                        <dx:ASPxGridViewExporter ID="GridExporter6" runat="server" GridViewID="grdHidden" />

                    </div>
                </div>
                <asp:HiddenField ID="hdWhichButton" runat="server" />
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
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <script type="text/javascript">
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
        function onlyAlphabets(evt) {
            var charCode;
            if (window.event)
                charCode = window.event.keyCode;  //for IE
            else
                charCode = evt.which;  //for firefox
            if (charCode == 32) //for &lt;space&gt; symbol
                return true;
            if (charCode > 31 && charCode < 65) //for characters before 'A' in ASCII Table
                return false;
            if (charCode > 90 && charCode < 97) //for characters between 'Z' and 'a' in ASCII Table
                return false;
            if (charCode > 122) //for characters beyond 'z' in ASCII Table
                return false;
            return true;
        }
        function isNumberandTextValidTE(evt) {
            debugger;
            if (isNumber(evt) == false) {
                tP_KeyPress(s, e)
            }


        }

    </script>
</asp:Content>
