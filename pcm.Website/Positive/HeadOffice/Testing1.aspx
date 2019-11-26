<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="Testing1.aspx.vb" Inherits="pcm.Website.WebForm1" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../../js/General/application.js"></script>
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

        .button1 {
            height: 1px;
            width: 2px;
        }

        .button2 {
            position: absolute;
            top: 172px;
            left: 266px;
        }
    </style>
    <script type="text/javascript">
        function onEnd(s, e) {
            lp.Hide();
        }
        function search(s, e) {
            e.processOnServer = false;
            cboSpecial.ClearItems();    
            detail.SetText("");
            limit.SetText("");
            cboSpecial.AddItem("Special Name", "sp");
            ASPxPopupControl1.Show();

        }
        function search1(s, e) {
            e.processOnServer = false;
            cboSpecial.ClearItems();         
            detail.SetText("");
            limit.SetText("");
            cboSpecial.AddItem("Master Code", "mc");            
            cboSpecial.AddItem("Generated Code", "gc");
            //cboSpecial.AddItem("Barcode", "bc");
            //cboSpecial.AddItem("Description", "ds");
            ASPxPopupControl1.Show();
           
        }
        function SpecialName(s, e) {
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "specialname";
            lp.Show();
            cab.PerformCallback();
        }
        function getRowvalue(s, e) {
            debugger;    
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "Rowvalue";
            ASPxPopupControl1.Hide()   
            lp.Show();
            cab.PerformCallback();
        }
        function clear(s, e) {
             e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "clear";
             lp.Show();
            cab.PerformCallback();
        }
        function AddData(s, e) {
            //debugger;
            e.processOnServer = false;
            document.getElementById('<%=hdWhichButton.ClientID%>').value = "AddData";
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
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px" ClientInstanceName="cab">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>
        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" ClientInstanceName="lp" Modal="true" ContainerElementID=""></dx:ASPxLoadingPanel>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssClass="date_panel" ShowCollapseButton="false" Width="500px">
                    <PanelCollection>
                        <dx:PanelContent>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Special Name"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Height="28px" Width="300px" ClientInstanceName="ASPxTextBox1">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="button1">
                                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="..." ClientInstanceName="spname">
                                            <ClientSideEvents Click="search" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Start Date"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server"></dx:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="End Date"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server"></dx:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Active"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server"></dx:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Price"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ASPxTextBox4" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Code"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ASPxTextBox5" runat="server" Height="28px" Width="170px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="button2">
                                        <dx:ASPxButton ID="ASPxButton3" runat="server" Text="...">
                                            <ClientSideEvents Click="search1" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Qty"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ASPxTextBox6" runat="server" Width="170px"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                         <dx:ASPxButton ID="ASPxButton6" runat="server" Text="Clear" >  
                                             <ClientSideEvents Click="clear" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td class="button">
                                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ClientSideEvents-Click="AddData">
                                        <ClientSideEvents Click=""></ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                &nbsp;
            <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="false" Width="900px" CssClass="date_panel" >
                <SettingsAdaptivity>
                    <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                </SettingsAdaptivity>
                
                <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="special_name" Caption="Stockcode" />
                    <dx:GridViewDataTextColumn FieldName="description" Caption="Description" />
                    <dx:GridViewDataTextColumn FieldName="qty" Caption="Qty" />
                    <dx:GridViewCommandColumn ShowDeleteButton="true" Caption="Action"></dx:GridViewCommandColumn>
                </Columns>
            </dx:ASPxGridView>
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                          <dx:ASPxButton ID="ASPxButton7" runat="server" Text="Save"></dx:ASPxButton>
                        </td>
                    </tr>                
                </table>                        
                <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ShowCloseButton="true" CloseAction="None" PopupAnimationType="Fade"
                    ClientInstanceName="ASPxPopupControl1" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="548px">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>    
                    <SettingsLoadingPanel Enabled="false" />
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Search Type"></dx:ASPxLabel>s
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboSpecial" ClientInstanceName="cboSpecial" runat="server" ValueType="System.String" >

                                        </dx:ASPxComboBox>                                      
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Search Details"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Width="170px" ClientInstanceName="detail"></dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="ASPxButton4" runat="server" Text="Search">
                                            <ClientSideEvents Click="SpecialName" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Limit"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="28px" Height="20px" ClientInstanceName="limit"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>

                            <dx:ASPxGridView ID="ASPxGridView2" runat="server" Width="500px" AutoGenerateColumns="true"  ViewStateMode="Enabled"
                                 SettingsBehavior-AllowSelectByRowClick="true" ClientInstanceName="grid"   Visible="true">                                                                                                                        
                                <SettingsPager Position="Bottom"></SettingsPager>
                                <SettingsAdaptivity>
                                <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                                </SettingsAdaptivity>

                                 <SettingsBehavior AllowSelectByRowClick="True"></SettingsBehavior>

                                <SettingsLoadingPanel Mode="ShowAsPopup" />
                                <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>
                                <Styles>
                                <SelectedRow BackColor="SlateGray"></SelectedRow>
                                </Styles>
                            </dx:ASPxGridView>
                            <%--<dx:ASPxGridView ID="ASPxGridView3" runat="server" Width="500px" AutoGenerateColumns="true"  ViewStateMode="Enabled"
                                Styles-SelectedRow-BackColor="WhiteSmoke" SettingsBehavior-AllowSelectByRowClick="true" ClientInstanceName="grid"   Visible="true">                                                                                                                        
                                <SettingsPager Position="Bottom"></SettingsPager>
                                <SettingsAdaptivity>
                                <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                                </SettingsAdaptivity>
                                <SettingsLoadingPanel Mode="Disabled" />
                                <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>
                                <Styles>
                                <SelectedRow BackColor="SlateGray"></SelectedRow>
                                </Styles>
                            </dx:ASPxGridView>--%>
                            <table> 
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="ASPxButton5" runat="server" Text="Select" AutoPostBack="false">
                                          <ClientSideEvents Click="getRowvalue" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <asp:HiddenField ID="hdWhichButton" runat="server" />               
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
