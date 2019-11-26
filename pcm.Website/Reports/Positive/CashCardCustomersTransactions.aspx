<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="CashCardCustomersTransactions.aspx.vb" Inherits="pcm.Website.CashCardCustomersTransactions" %>

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
        HeaderText="Cash Card Customers Transactions" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="From Date:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txtFromDate" runat="server" Font-Bold="True" Width="250px" Date="04/09/2013 16:21:55" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" UseMaskBehavior="True">
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
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Customer Account:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>

                            <dx:ASPxTextBox ID="txtAccount" Style="float: left;" runat="server" Width="170px">
                            </dx:ASPxTextBox>
                            &nbsp;
                                <dx:ASPxImage ID="imgQ" runat="server" ImageUrl="~/images/search.png"></dx:ASPxImage>

                        </td>
                        <td></td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                           
                        </td>

                    </tr>
                    <tr>
                        <td>   <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Customer Name:" Width="120px">
                            </dx:ASPxLabel></td>
                        <td> <dx:ASPxTextBox ID="txtCustomerName" runat="server" Style="float: left;" Width="170px">
                            </dx:ASPxTextBox></td>
                        <td>&nbsp;</td>
                        <td><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Opened At:" Width="120px">
                            </dx:ASPxLabel>&nbsp;</td>
                        <td> <dx:ASPxTextBox ID="txtOpenedAt" runat="server" Style="float: left;" Width="170px">
                            </dx:ASPxTextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Contact Number:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtContactNumber" runat="server" Style="float: left;" Width="170px">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="EMail:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtEMail" runat="server" Style="float: left;" Width="170px">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Date Opened:" Width="120px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtDateOpened" runat="server" Style="float: left;" Width="170px">
                            </dx:ASPxTextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td><dx:ASPxButton ID="cmdRun" Style="float: right; margin-left: 0px;" runat="server" Text="Run"></dx:ASPxButton></td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="526px" Height="250px"
                    MaxWidth="800px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="pcMain"
                    ShowFooter="True" FooterText="" PopupElementID="imgQ" HeaderText="Cash Customer Search"
                    runat="server" PopupHorizontalAlign="Center" EnableHierarchyRecreation="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <asp:Panel ID="Panel1" runat="server">
                                <table border="0" cellpadding="4" cellspacing="0">

                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Last Name"></dx:ASPxLabel>
                                                    </td>
                                                    <td>

                                                        <dx:ASPxTextBox ID="txtAccountSearch" runat="server" Width="200px">
                                                            <ClientSideEvents KeyUp="function(s, e) {s.SetText(s.GetText().toUpperCase());}" />
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <dx:ASPxButton ID="cmdSearch" runat="server" Text="Search">
                                                        </dx:ASPxButton>
                                                    </td>

                                                </tr>

                                            </table>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">
                                            <dx:ASPxGridView ID="grdAccountNumber" OnDataBinding="grdAccountNumber_DataBinding" runat="server" AutoGenerateColumns="False" Width="425px">


                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="account_number" Caption="Account Number" VisibleIndex="1" />
                                                    <dx:GridViewDataTextColumn FieldName="account_name" Caption="Account Name" VisibleIndex="2" />
                                                </Columns>


                                                <SettingsBehavior AllowSelectByRowClick="true" EnableRowHotTrack="True" AllowSort="False" />
                                                <SettingsPager PageSize="20">
                                                </SettingsPager>

                                            </dx:ASPxGridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">&nbsp;<dx:ASPxButton ID="cmdSelect" runat="server" Text="Select" Width="425">
                                        </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseUp="function(s, e) { SetImageState(false); }" PopUp="function(s, e) { SetImageState(true); }" />
                </dx:ASPxPopupControl>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

    <dx:ASPxGridView ID="dxGrid" runat="server" CssClass="date_panel" 
        AutoGenerateColumns="False" OnDataBinding="dxGrid_DataBinding" Width="98%">
        <Columns>
            <dx:GridViewDataTextColumn Caption="Trans Date" FieldName="sale_date" 
                VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Trans Time" FieldName="sale_time" 
                VisibleIndex="1">
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code" 
                VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Account Number" FieldName="account_number" 
                VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Trans Type" FieldName="transaction_type" 
                VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Trans Number" FieldName="transaction_number" 
                VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Trans Total" FieldName="transaction_total" 
                VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Trans Points" FieldName="transaction_points" 
                VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            
        </Columns>
        <SettingsPager PageSize="50">
        </SettingsPager>
        <Settings ShowFilterRow="True" ShowGroupedColumns="True" 
            ShowGroupPanel="True" />
        <Settings ShowFooter="True" />
         <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="Transaction Total: {0}" FieldName="transaction_total" SummaryType="Sum" 
                />
            <dx:ASPxSummaryItem DisplayFormat="Transaction Points: {0}" FieldName="transaction_points" SummaryType="Sum" />
            
            <%--<dx:ASPxSummaryItem FieldName="totallengthofcalls" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="averagelengthofcalls" SummaryType="Average" />--%>
        </TotalSummary>
    </dx:ASPxGridView>

     <table>
    <tr>
    <td>
       <dx:ASPxButton ID="cmdExportPDF" runat="server" Text="Export to PDF"  Width="164px" CssClass="date_panel">
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
