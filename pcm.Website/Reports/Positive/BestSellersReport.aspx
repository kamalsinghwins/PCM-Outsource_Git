<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="BestSellersReport.aspx.vb" Inherits="pcm.Website.BestSellersReport" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../../js/General/application.js"></script>
    <script type="text/javascript" src="../../../js/Collections/contactinvestigation.js"></script>
    <link href="css/new_style.css" rel="stylesheet" />
    <style type="text/css">
        .mainContainer {
            padding: 15px 20px;
        }

        .mb-20 {
            margin-bottom: 20px;
        }

        .ellipsis {
            text-overflow: ellipsis;
            overflow: hidden;
            max-width: 100px !important;
        }

        .mr-20 {
            margin-right: 20px;
        }

        .mt-20 {
            margin-top: 20px;
        }

        .text-transform-capitalize {
            text-transform: capitalize;
        }

        .auto-style2 {
            width: 140px;
        }
    </style>

    <script type="text/javascript">

        function GetReports(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "cmdRun";

            lp.Show();
            cab.PerformCallback();


        }

        function onEnd(s, e) {
            lp.Hide();

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
        OnCallback="ASPxCallback1_Callback">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>
        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">

                <div class="mainContainer">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px" Style="float: left;"
                        HeaderText="Best Sellers Report">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                                    ClientInstanceName="lp">
                                </dx:ASPxLoadingPanel>
                                  <table>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:"  Width="120px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True"  Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                </dx:ASPxDateEdit>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="txtToDate" runat="server" Font-Bold="True"  Width="250px" Date="04/09/2013 16:22:30"
                                    DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                                </dx:ASPxDateEdit>
                            </td>
                            <td></td>

                        </tr>

                        <tr>
                            <td></td>
                            <td>
                                <dx:ASPxCheckBox ID="chkMastercode" runat="server" CheckState="Unchecked" Text="Mastercode" >
                                </dx:ASPxCheckBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td>
                                <dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run">
                                    <ClientSideEvents Click ="GetReports" />
                                </dx:ASPxButton>
                            </td>

                        </tr>
                    </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <br />
                    <br />
                       <dx:ASPxGridView ID="gvMaster" Style="margin-top :20px;display:inline-block" runat="server" AutoGenerateColumns="False" OnDataBinding="gvMaster_DataBinding"
            EnableTheming="True" Width="900px">

            <Columns>
                <dx:GridViewDataTextColumn FieldName="stockcode" Caption="Stockcode" VisibleIndex="0" Width="180px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="description" Caption="Description" VisibleIndex="1" Width="300px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" VisibleIndex="2" Width="100px">
                    <PropertiesTextEdit DisplayFormatString="f" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="estimated_calc" Caption="GP% on Est." VisibleIndex="3" UnboundExpression="(selling-estimated)/selling" UnboundType="Decimal" Width="140px">
                    <PropertiesTextEdit DisplayFormatString="p" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="average_calc" Caption="GP% on Avg." VisibleIndex="4" UnboundExpression="(selling-average)/selling" UnboundType="Decimal" Width="140px">
                    <PropertiesTextEdit DisplayFormatString="p" />
                </dx:GridViewDataTextColumn>
            </Columns>
            <Templates>
                <DetailRow>
                    <dx:ASPxGridView ID="gvDetail" runat="server"
                        Width="100%" OnDataBinding="gvDetail_DataBinding"  AutoGenerateColumns="False">
                        <Columns>
                            <dx:GridViewDataColumn FieldName="sale_date" Caption="Sale Date" VisibleIndex="1" />

                            <dx:GridViewDataColumn FieldName="branch_name" Caption="Branch Name" VisibleIndex="3" />
                            <dx:GridViewDataColumn FieldName="transaction_type" VisibleIndex="4" Caption="Transaction Type" />

                            <dx:GridViewDataTextColumn FieldName="quantity" VisibleIndex="5" Caption="Quantity">
                                <PropertiesTextEdit DisplayFormatString="f" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="selling_price" VisibleIndex="6" Caption="Selling Price Total">
                                <PropertiesTextEdit DisplayFormatString="f" />
                            </dx:GridViewDataTextColumn>

                        </Columns>
                        <Settings ShowFooter="True" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="quantity" SummaryType="Sum" />
                            <dx:ASPxSummaryItem FieldName="selling_price" SummaryType="Sum" />

                        </TotalSummary>
                        <Settings ShowGroupPanel="True" />
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                    </dx:ASPxGridView>
                </DetailRow>
            </Templates>
            <%--<TotalSummary>
                    <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" />
                </TotalSummary>--%>
            <SettingsBehavior ColumnResizeMode="Control" />

            <SettingsPager PageSize="20">
            </SettingsPager>

            <SettingsDetail AllowOnlyOneMasterRowExpanded="False" ShowDetailRow="True" ExportMode="Expanded" />
        </dx:ASPxGridView>
                </div>
                 
    <table>
        <tr>
            <td>
                <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF" Width="164px" CssClass="date_panel">
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="cmdExportExcel" runat="server" Text="Export to Excel" Width="164px" CssClass="date_panel">
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="cmdExportCSV" runat="server" Text="Export to CSV" Width="164px" CssClass="date_panel">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <dx:ASPxGridViewExporter ID="Exporter" runat="server">
    </dx:ASPxGridViewExporter>
                <br />
  
                <asp:HiddenField ID="hdWhichButton" runat="server" />
                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Error" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2">
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="There was an error updating this account. Please contact support."
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
</asp:Content>
