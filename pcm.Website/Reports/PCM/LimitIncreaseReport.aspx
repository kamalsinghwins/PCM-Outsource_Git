﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="LimitIncreaseReport.aspx.vb" Inherits="pcm.Website.LimitIncreaseReport" %>

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
        HeaderText="Limit Increase Report" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td>
                            <dx:ASPxLabel ID="lblFromDate" runat="server" Text="From Date:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="lblToDate" runat="server" Text="To Date:" Width="90px">
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



    <dx:ASPxGridView ID="gvLimitIncreaseReport" OnDataBinding="gvMaster_DataBinding" runat="server" AutoGenerateColumns="False" CssClass="date_panel" Width="98%"
        EnableTheming="True">

        <Columns>
            <dx:GridViewDataTextColumn Caption="Account" FieldName="account_number"
                VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Previous Limit" FieldName="previous_limit"
                VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="New Limit" FieldName="new_limit"
                VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Limit Increased" FieldName="limit_increased" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Current Balance" FieldName="current_balance"
                VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Payments" FieldName="total_spent"
                VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Income Amount" FieldName="income_amount"
                VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="SMS Sent" FieldName="number_sms_sent"
                VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Notes" FieldName="additional_notes"
                VisibleIndex="9">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="SMS Reply" FieldName="sms_reply"
                VisibleIndex="10">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Date Of Run" FieldName="date_of_run"
                VisibleIndex="11">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Auto Increase" FieldName="auto_increase"
                VisibleIndex="12">
            </dx:GridViewDataTextColumn>
        </Columns>
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="previous_limit" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="new_limit" SummaryType="Sum" />

        </TotalSummary>

        <SettingsBehavior AllowEllipsisInText="true" />
        <SettingsBehavior ColumnResizeMode="NextColumn" />

        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFooter="True" />
        <SettingsPager PageSize="100">
        </SettingsPager>

        <Settings ShowFooter="True" />

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
