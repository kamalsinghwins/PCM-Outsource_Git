﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InventoryAndTax.ascx.vb" Inherits="pcm.Website.InventoryAndTax" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<table>
    <tr>
        <td colspan="3">
            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Inventory" Style="margin-bottom: 10px"
                Font-Bold="True" Font-Names="Verdana" ForeColor="#0066FF">
            </dx:ASPxLabel>
            <br />
            <dx:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="~/images/blue-line.png" Style="width: 218px;
                margin-bottom: 6px">
            </dx:ASPxImage>
        </td>
        <td class="middleSeparator">
            &nbsp;
        </td>
        <td colspan="3">
            <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Tax" Style="margin-bottom: 10px"
                Font-Bold="True" Font-Names="Verdana" ForeColor="#0066FF">
            </dx:ASPxLabel>
            <br />
            <dx:ASPxImage ID="ASPxImage4" runat="server" ImageUrl="~/images/blue-line.png" Style="width: 218px;
                margin-bottom: 6px">
            </dx:ASPxImage>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Stockcode:" Width="120px">
            </dx:ASPxLabel>
        </td>
        <td>
            <dx:ASPxTextBox ID="txtStockcode" CssClass="dxeTextBox" runat="server" Width="170px">
                <ClientSideEvents KeyPress="CodeRegEx" />
            </dx:ASPxTextBox>
        </td>
        <td>
            <dx:ASPxImage ID="imgQ" runat="server" ImageUrl="~/images/search.png">
            </dx:ASPxImage>
        </td>
        <td class="middleSeparator">
            &nbsp;
        </td>
        <td>
            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Purchase Tax:" Width="120px">
            </dx:ASPxLabel>
        </td>
        <td colspan="2">
            <dx:ASPxComboBox ID="cboPurchaseTax" ClientInstanceName="purchasetax" runat="server"
                ValueType="System.String">
                <Columns>
                    <dx:ListBoxColumn Caption="Select Tax Group" FieldName="taxgroups" />
                </Columns>
                <ClientSideEvents SelectedIndexChanged="purchasetaxupdated" />
            </dx:ASPxComboBox>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Barcode:" Width="120px">
            </dx:ASPxLabel>
        </td>
        <td colspan="2">
            <dx:ASPxTextBox ID="txtBarcode" CssClass="dxeTextBox" runat="server" Width="170px">
                <ClientSideEvents KeyPress="BarcodeRegEx" />
            </dx:ASPxTextBox>
        </td>
        <td class="middleSeparator">
            &nbsp;
        </td>
        <td>
            <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Sales Tax:" Width="120px">
            </dx:ASPxLabel>
        </td>
        <td colspan="2">
            <dx:ASPxComboBox ID="cboSalesTax" ClientInstanceName="salestax" runat="server" ValueType="System.String">
                <Columns>
                    <dx:ListBoxColumn Caption="Select Tax Group" FieldName="taxgroups" />
                </Columns>
                <ClientSideEvents SelectedIndexChanged="saletaxupdated" />
            </dx:ASPxComboBox>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Description:" Width="120px">
            </dx:ASPxLabel>
        </td>
        <td>
            <dx:ASPxTextBox ID="txtDescription" runat="server" CssClass="dxeTextBox" Width="170px">
                <ClientSideEvents KeyPress="DescriptionRegEx" />
            </dx:ASPxTextBox>
        </td>
        <td class="middleSeparator" colspan="5">
            &nbsp;
        </td>
    </tr>
</table>

