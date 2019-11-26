<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="AccountsOpenedByEmployee.aspx.vb" Inherits="pcm.Website.AccountsOpenedByEmployee" %>


<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .date_panel {
        }
    </style>
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

                <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="229px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="400px">
                </dx:ASPxDockZone>

            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">

    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px"
        HeaderText="Accounts Opened By Employee" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="From Date:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px"  DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="To Date:" Width="90px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtToDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:22:30"
                                DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td></td>

                    </tr>

                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>
                            <dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run"></dx:ASPxButton>
                        </td>

                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>



    <dx:ASPxGridView ID="gvMaster" runat="server" AutoGenerateColumns="False" OnDataBinding="gvMaster_DataBinding" CssClass="date_panel" Width="98%"
        EnableTheming="True">

        <Columns>
            <dx:GridViewDataTextColumn Caption="Employee #" FieldName="employee_number"
                VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="First Name" FieldName="first_name"
                VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Last Name" FieldName="last_name"
                VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Accounts Opened" FieldName="total_opened" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Active Accounts" FieldName="total_active"
                VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Commission Accounts" FieldName="total_spent"
                VisibleIndex="5">
            </dx:GridViewDataTextColumn>
        </Columns>
        <Templates>
            <DetailRow>
                <dx:ASPxGridView ID="gvDetail" runat="server"
                    Width="100%" OnDataBinding="gvDetail_DataBinding" AutoGenerateColumns="False">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="account_number" Caption="Account #" VisibleIndex="1" />
                        <dx:GridViewDataColumn FieldName="first_name" Caption="First Name" VisibleIndex="2" />
                        <dx:GridViewDataColumn FieldName="last_name" VisibleIndex="3" Caption="Last Name" />
                        <dx:GridViewDataTextColumn FieldName="status" VisibleIndex="4" Caption="Account Status">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="spent" VisibleIndex="5" Caption="Spent">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="sent_to" VisibleIndex="6" Caption="Sent To">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="first_purchase" VisibleIndex="7" Caption="Branch Code">
                        </dx:GridViewDataTextColumn>

                    </Columns>
                    <Settings ShowFooter="True" />
                    <GroupSummary>
                        <dx:ASPxSummaryItem DisplayFormat="Accounts Opened: {0}" FieldName="total_opened" SummaryType="Sum" />

                    </GroupSummary>
                    <Settings ShowGroupPanel="True" ShowFilterRow="true" />
                    <SettingsPager PageSize="100">
                    </SettingsPager>
                </dx:ASPxGridView>
            </DetailRow>
        </Templates>
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="Accounts Opened: {0}" FieldName="total_opened" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="Active Accounts: {0}" FieldName="total_active" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="Commission Accounts: {0}" FieldName="total_spent" SummaryType="Sum" />
            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </TotalSummary>
        <%--<TotalSummary>
                    <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" />
                </TotalSummary>--%>
        <SettingsBehavior ColumnResizeMode="Control" />

        <Settings ShowGroupPanel="True" ShowFilterRow="true" />
        <SettingsPager PageSize="100">
        </SettingsPager>

        <Settings ShowFooter="True" />

        <SettingsDetail AllowOnlyOneMasterRowExpanded="False" ShowDetailRow="True" ExportMode="Expanded" />
    </dx:ASPxGridView>


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
</asp:Content>

