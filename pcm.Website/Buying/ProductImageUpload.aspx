﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="ProductImageUpload.aspx.vb" Inherits="pcm.Website.ProductImageUpload" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>








<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/StockcodeManager/InventoryAndTax.ascx" TagName="InventoryAndTax"
    TagPrefix="widget" %>



<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/collections.css" rel="stylesheet" />
    <script src="../../js/General/application.js"></script>
     <script type="text/JavaScript" language="JavaScript">
         function openwindow() {
             window.open("Product_Preview.aspx?code=" + txtStockcode.GetText(), "mywindow", "menubar=0,scrollbars=yes,resizable=1,width=1024,height=600");
         }
    </script>
    <script type="text/javascript">
        // <![CDATA[
       
        function ShowMessage(message) {
            window.setTimeout("alert('" + message + "')", 0);
        }
       
        function onSelectClick(s, e) {
            e.processOnServer = false;
            hdDEWhichButton.Set("clicked", "Select");

            lp.Show();
            cab.PerformCallback();


        }

        function onSearchClick(s, e) {
            e.processOnServer = false;

            hdDEWhichButton.Set("clicked", "Search");

            lp.Show();
            cab.PerformCallback();


        }

        function onEditPicture(s, e) {
            e.processOnServer = false;

            hdDEWhichButton.Set("filetoupload", UploadControl.GetText(0));

            hdDEWhichButton.Set("clicked", "EditPicture");

            lp.Show();
            cab.PerformCallback();


        }

        function StockcodeLostFocus(s, e) {
            e.processOnServer = false;

            var stockcode = txtStockcode.GetText();
            if (stockcode == '') {
                return false;
            }


            hdDEWhichButton.Set("clicked", "StockcodeLostFocus");

            lp.Show();
            cab.PerformCallback();


        }

        function onClearClick(s, e) {
            e.processOnServer = false;

            hdDEWhichButton.Set("clicked", "Clear");

            lp.Show();
            cab.PerformCallback();
        }

        function onUpload(s, e) {
            e.processOnServer = false;

            hdDEWhichButton.Set("clicked", "Upload");

            lp.Show();
            cab.PerformCallback();
        }

        function onEnd(s, e) {
            lp.Hide();

        }
        // ]]> 
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
                            <widget:DateTime ID="DateTimeWidget" runat="server" />
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
    <div>

            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cab"
                OnCallback="ASPxCallback1_Callback" SettingsLoadingPanel-Enabled="False">
                <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                         <asp:ScriptManager runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
                        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                            ClientInstanceName="lp">
                        </dx:ASPxLoadingPanel>
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="500"
                            HeaderText="Upload Product Images" CssClass="date_panel">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                    <table>
                                        <tr>
                                            <td style="padding-right: 20px; vertical-align: top;">
                                                <table>
                                                    <tr>

                                                        <td>

                                                            <dx:ASPxTextBox ID="txtStockcode" Style="width: 300px; float: left;" runat="server"
                                                                NullText="Stockcode with Colour" CssClass="UpperCase" ClientInstanceName="txtStockcode">
                                                                <ClientSideEvents LostFocus="StockcodeLostFocus" />
                                                            </dx:ASPxTextBox>
                                                            <dx:ASPxImage ID="imgQ" runat="server" Style="margin-left: 10px;" ImageUrl="~/images/search.png">
                                                            </dx:ASPxImage>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtMaterials" Style="width: 330px; float: left;" runat="server" NullText="Materials" ClientInstanceName="txtMaterials">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtQtyOrdered" Style="width: 330px; float: left;" runat="server" NullText="Quantity Ordered" ClientInstanceName="txtQtyOrdered">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td><dx:ASPxTextBox ID="txtPrice" Style="width: 330px; float: left;" runat="server" NullText="Price" ClientInstanceName="txtPrice">
                                                        <ClientSideEvents KeyPress="NumericOnly" />    
                                                        </dx:ASPxTextBox></td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <dx:ASPxMemo ID="txtDescription" Style="width: 330px; float: left;" NullText="Short Description" runat="server" Height="71px" MaxLength="1000">
                                                            </dx:ASPxMemo>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                            <dx:ASPxComboBox ID="cboCategory" Style="width: 330px;" NullText="Category" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                                                           </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                          
                                                           <a href="javascript: openwindow()">Product Preview</a>
                                                           
                                                          
                                                        </td>
                                                    </tr>

                                                    
                                                    <tr>
                                                        <td>
                                                           <dx:ASPxTextBox ID="txtDisplayOrder" Style="width: 330px; float: left;" runat="server" NullText="Display Order" ClientInstanceName="cboDisplayOrder">
                                                        <ClientSideEvents KeyPress="NumericOnly" />    
                                                        </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>

                                                    
                                                   
                                                    
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>

                                                    
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="cmdUpload" runat="server" Style="float: left;" Text="Upload"
                                                                AutoPostBack="false" Width="330px">
                                                                <ClientSideEvents Click="onUpload"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                           
                                                        </td>
                                                    </tr>
                                                  

                                                    <tr>
                                                        <td class="note">
                                                            <dx:ASPxButton ID="cmdClear" runat="server" Style="float: left;" Text="Clear" AutoPostBack="false">
                                                                <ClientSideEvents Click="onClearClick"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td class="note">&nbsp;</td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                             <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="Info" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AppearAfter="100"
                    DisappearAfter="1000" PopupAnimationType="Fade">
                    <ClientSideEvents CloseButtonClick="fadeOut"></ClientSideEvents>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div>
                                <div id="Div2">
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="The Product catalogue was updated."
                                        Font-Size="16px">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl>
                                                            <dx:ASPxPopupControl ClientInstanceName="StockSearch" Width="526px" Height="250px"
                                                                MaxWidth="800px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="pcMain"
                                                                ShowFooter="True" FooterText="" PopupElementID="imgQ" HeaderText="Stockcode Search"
                                                                runat="server" PopupHorizontalAlign="WindowCenter" EnableHierarchyRecreation="True">
                                                                <ContentCollection>
                                                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                                                                        <asp:Panel ID="Panel1" runat="server">
                                                                            <table border="0" align="left" cellpadding="4" cellspacing="0">
                                                                                <tr>

                                                                                    <td>
                                                                                        <dx:ASPxTextBox ID="txtStockcodeSearch" Height="25px" Style="float: left; margin-right: 10px;"
                                                                                            CssClass="UpperCase" runat="server" Width="250px">
                                                                                        </dx:ASPxTextBox>
                                                                                        <dx:ASPxButton ID="cmdSearch" runat="server" Text="Search" AutoPostBack="false">
                                                                                            <ClientSideEvents Click="onSearchClick"></ClientSideEvents>
                                                                                        </dx:ASPxButton>
                                                                                    </td>
                                                                                </tr>

                                                                                <tr>
                                                                                    <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">
                                                                                        <dx:ASPxGridView ID="grdStockcodeSearch" runat="server" AutoGenerateColumns="False"
                                                                                            OnDataBinding="grdStockcodeSearch_DataBinding" Width="425px">
                                                                                            <Columns>
                                                                                                <dx:GridViewDataTextColumn FieldName="stockcode" Caption="Stockcode" VisibleIndex="1" />
                                                                                            </Columns>
                                                                                            <SettingsBehavior AllowSelectByRowClick="true" EnableRowHotTrack="True" AllowSort="False" />
                                                                                            <SettingsBehavior AllowSort="False" AllowSelectByRowClick="True" EnableRowHotTrack="True"></SettingsBehavior>
                                                                                        </dx:ASPxGridView>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="color: #666666; font-family: Tahoma; font-size: 14px;" valign="top">&nbsp;
                                                                <dx:ASPxButton ID="cmdSelect" runat="server" Text="Select" Width="425" AutoPostBack="false" ClientInstanceName="SelectStockcode">
                                                                    <ClientSideEvents Click="onSelectClick"></ClientSideEvents>
                                                                </dx:ASPxButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </dx:PopupControlContentControl>
                                                                </ContentCollection>
                                                            </dx:ASPxPopupControl>
                                                        </td>

                                                    </tr>
                                                </table>
                                            </td>

                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                       
                        <asp:HiddenField ID="hdWhichButton" runat="server" />

                                    <dx:ASPxHiddenField ID="hdDEWhichButton" ClientInstanceName="hdDEWhichButton" runat="server">
                                    </dx:ASPxHiddenField>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
         <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Width="500"
                            HeaderText="" CssClass="date_panel">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                 <table>
            <tr>
                <td>
                    <ccPiczardUC:SimpleImageUpload ID="picUpload" Width="330px" runat="server" BackColor="#EAF1FA" />
                </td>
            </tr>
        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
             </dx:ASPxRoundPanel>
       
         
        </div>
</asp:Content>
