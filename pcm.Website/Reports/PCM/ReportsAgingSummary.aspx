<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ReportsAgingSummary.aspx.vb" Inherits="pcm.Website.ReportsAgingSummary" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .date_panel {
        }
        
    </style>
    <script type="text/javascript">
        function onStart(s, e) {
            ASPxLoadingPanel1.Hide();
        }
        function OnClick(s, e) {
            if (!ASPxClientEdit.ValidateGroup("cmdRun")) return;
                document.getElementById('<%=ASPxHiddenField1.ClientID%>').value = "cmdRun";
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

                <dx:ASPxDockZone ID="ASPxDockZone1" runat="server" Width="229px" ZoneUID="zone1"
                    PanelSpacing="3px" ClientInstanceName="splitter" Height="400px">
                </dx:ASPxDockZone>

            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px" ClientInstanceName="ASPxCallbackPanel1">
       <PanelCollection>  
           <dx:PanelContent ID="PanelContent1">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="500px"
        HeaderText="User Transactions Summary" CssClass="date_panel">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                <table>

                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Total Outstanding >= " Width="152px">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <dx:ASPxTextBox ID="txtAmount" runat="server" Width="170px" Text="0">
                            </dx:ASPxTextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                                         
                    </tr>

                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <dx:ASPxButton ID="cmdRun" runat="server" Style="float: right; margin-left: 0px;" Text="Run">
                                <ClientSideEvents Click="OnClick" />
                            </dx:ASPxButton>
                        </td>                     
                    </tr>   
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ClientInstanceName="ASPxLoadingPanel1">
    </dx:ASPxLoadingPanel>
    <br />
    <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="date_panel" Text="Outstanding Amounts"></dx:ASPxLabel>
    <dx:ASPxGridView ID="dxGrid" runat="server" CssClass="date_panel" 
        AutoGenerateColumns="true" OnDataBinding="dxGrid_DataBinding" Width="900px" >
  

    </dx:ASPxGridView>

    <br />
    <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="date_panel" Text="Number of Customers to Call"></dx:ASPxLabel>

     <dx:ASPxGridView ID="dxGridCalls" runat="server" CssClass="date_panel" 
        AutoGenerateColumns="True" OnDataBinding="dxGrid_DataBinding_Calls" Width="900px">
        
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
    <br />
    <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="ASPxHiddenField1">
    </dx:ASPxHiddenField>
         </dx:PanelContent>
        </PanelCollection>
        </dx:ASPxCallbackPanel>
</asp:Content>
