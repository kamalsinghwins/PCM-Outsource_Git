﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Password_Reset.aspx.vb" Inherits="pcm.Website.Password_Reset" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Password Reset</title>
    <script src="../js/General/application.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table align="center">
          <tr>
              
             <td>
                 <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Please fill-in the email address associated with your account:" Theme="RedWine"></dx:ASPxLabel>
             </td>
              </tr>
          <tr>
              <td>
                  <dx:ASPxTextBox ID="txtEmailAddress" runat="server" Width="562px" Theme="iOS"></dx:ASPxTextBox>
              </td>
          </tr>
          <tr>
              <td>
                  &nbsp;</td>
          </tr>
          <tr>
              <td>
                  <dx:ASPxButton ID="cmdReset" style="float:right" runat="server" Text="Reset" Theme="iOS">
                  </dx:ASPxButton>
              </td>
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
                                    <dx:ASPxLabel ID="lblError" runat="server" Text="We could not find this email address in our database"
                                        Font-Size="16px">
                                    </dx:ASPxLabel>
                                    <br />
                                    <br />

                                   
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <ClientSideEvents CloseButtonClick="fadeOut" />
                </dx:ASPxPopupControl></td>
          </tr>
          </table>
    </div>
    </form>
</body>
</html>
