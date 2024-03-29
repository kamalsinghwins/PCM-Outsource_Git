﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="listing.aspx.vb" Inherits="pcm.Website.listing" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Widgets/wid_datetime.ascx" TagName="DateTime" TagPrefix="widget" %>
<%@ Register Src="~/Widgets/CallsForToday.ascx" TagName="CallsForToday" TagPrefix="widget" %>

<%@ Register Assembly="DevExpress.Dashboard.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="../js/Collections/contactinvestigation.js"></script>
    <script type="text/javascript" src="../js/General/application.js"></script>

    <style type="text/css">
        .mainContainer {
            padding: 15px 20px;
        }

        .text-center {
            text-align: center;
        }
        .text-left {
            text-align: left;
        }
        .text-right {
            text-align: right;
        }
        .pull-left {
            float: left;
        }
        .pull-right {
            float: right;
        }
        .mb-10 {
            margin-bottom: 10px;
        }
        .mainContainer>table{width:100%;margin-top:20px;float:left;}
    </style>
    <script type="text/javascript">

        function FillDebtorsDetails(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "SurveySelected";

            cab.PerformCallback();
        }

        function SubmitForm(s, e) {
            debugger;
            e.processOnServer = false;
            //do validation here
            if (!ASPxClientEdit.ValidateGroup("ErrorGroup")) return;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "CreateSurvey";

            lp.Show();
            cab.PerformCallback();
        }

        function OpenAddSurveyPopup(s, e) {
            e.processOnServer = false;
            debugger;
            ASPxClientEdit.ClearEditorsInContainer(null);

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "OpenAddSurveyPopup";

            lp.Show();
            cab.PerformCallback();
        }

        function OpenEditSurveyPopup(s, e) {
            e.processOnServer = false;

            ASPxClientEdit.ClearEditorsInContainer(null);

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "OpenEditSurveyPopup";

            lp.Show();
            cab.PerformCallback();
        }

        function DeleteSurvey(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "DeleteSurvey";

            lp.Show();
            cab.PerformCallback();
        }

        function EditSurveyQuestions(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "EditSurveyQuestions";

            lp.Show();
            cab.PerformCallback();
        }

         function CopySurvey(s, e) {
            e.processOnServer = false;

            document.getElementById('<%=hdWhichButton.ClientID%>').value = "CopySurvey";

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
        OnCallback="ASPxCallback1_Callback" SettingsLoadingPanel-Enabled="False">
        <SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>

        <ClientSideEvents EndCallback="onEnd"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Modal="true" ContainerElementID=""
                    ClientInstanceName="lp">
                </dx:ASPxLoadingPanel>
                <div class="mainContainer">
                    <div class="buttons">
                        <div class="text-left pull-left">
                            <dx:ASPxButton ID="btnCreateSurvey" runat="server" Text="Create Questionnaire">
                                <ClientSideEvents Click="OpenAddSurveyPopup"></ClientSideEvents>
                            </dx:ASPxButton>
                        </div>
                        <div class="text-right pull-right">
                            <dx:ASPxButton ID="btnEdit" runat="server" Text="Edit Questionnaire">
                                <ClientSideEvents Click="OpenEditSurveyPopup"></ClientSideEvents>
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnEditQuestion" runat="server" Text="Edit Questions">
                                <ClientSideEvents Click="EditSurveyQuestions"></ClientSideEvents>
                            </dx:ASPxButton>
                             <dx:ASPxButton ID="btnCopy" runat="server" Text="Copy Questionnaire">
                                <ClientSideEvents Click="CopySurvey"></ClientSideEvents>
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete Questionnaire">
                                <ClientSideEvents Click="DeleteSurvey"></ClientSideEvents>
                            </dx:ASPxButton>
                        </div>
                    </div>
                    <dx:ASPxGridView ID="grdSurveyListing" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="survey_id" OnDataBinding="gridSurveyListing_DataBinding">
                        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="False" />

<EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Questionnaire Id" FieldName="survey_id" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Questionnaire name" FieldName="survey_name" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Type Of Questionnaire" FieldName="type_of_survey" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Active" FieldName="is_active" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Time Allowed (In Minutes)" FieldName="max_time_allowed" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Questions" FieldName="no_questions" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Created By" FieldName="created_by" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Created Date" FieldName="time_stamp" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>                        
                        </Columns>
<SettingsAdaptivity>
<AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
</SettingsAdaptivity>

                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <SettingsEditing EditFormColumnCount="3" />
                        <SettingsPager Mode="ShowAllRecords" />

                    </dx:ASPxGridView>
                </div>
                <dx:ASPxPopupControl ID="dxPopUpError" runat="server" ShowCloseButton="True" Style="margin-right: 328px"
                    HeaderText="" Width="548px" CloseAction="None" ClientInstanceName="dxPopUpError"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
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

                <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="700px" Height="250px"
                    MaxWidth="850px" MaxHeight="250px" MinHeight="250px" MinWidth="150px" ID="SurveyPopup"
                    ShowFooter="True" FooterText="" PopupElementID="btnCreateSurvey" HeaderText=""
                    runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True" AllowDragging="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <asp:Panel ID="Panel1" runat="server">
                                <div class="pull-left">
                                    <asp:HiddenField ID="HDSurveyId" runat="server" />
                                    <table id="tblContainer0">
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="txtNameOfSurveyLabel" runat="server" Text="Name Of Questionnaire">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxTextBox ID="txtNameOfSurvey" runat="server" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ErrorText="Please Enter Questionnaire Name" SetFocusOnError="True" ValidationGroup="ErrorGroup">
                                                        <RequiredField ErrorText="Please Enter Questionnaire Name" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                        </tr>

                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="txtTypeOfSurveyLabel" runat="server" Text="Type Of Questionnaire">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxComboBox ID="cboTypeOfSurvey" runat="server" ValueType="System.String">
                                                    <Items>
                                                        <dx:ListEditItem Text="Pre-Employment Test" Value="Pre-Employment Test" />
                                                        <dx:ListEditItem Text="Employee Test" Value="Employee Test" />
                                                    </Items>
                                                    <ValidationSettings Display="Dynamic" ErrorText="Please Select Type of Questionnaire" SetFocusOnError="True" ValidationGroup="ErrorGroup">
                                                        <RequiredField ErrorText="Please Select Type of Questionnaire" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">

                                                <dx:ASPxLabel ID="txtMaximumTimeAllowedLabel" runat="server" Text="Maximum Time Allowed">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">

                                                <dx:ASPxComboBox ID="cboTimeAllowed" runat="server" ValueType="System.String">
                                                    <Items>
                                                        <dx:ListEditItem Text="10 min" Value="10" />
                                                        <dx:ListEditItem Text="20 min" Value="20" />
                                                        <dx:ListEditItem Text="30 min" Value="30" />
                                                        <dx:ListEditItem Text="40 min" Value="40" />
                                                        <dx:ListEditItem Text="50 min" Value="50" />
                                                        <dx:ListEditItem Text="60 min" Value="60" />
                                                    </Items>
                                                    <ValidationSettings Display="Dynamic" ErrorText="Please Select Maximum Time" SetFocusOnError="True" ValidationGroup="ErrorGroup">
                                                        <RequiredField ErrorText="Please Select Maximum Time" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td class="style3"></td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <dx:ASPxLabel ID="chkIsActiveLabel" runat="server" Text="Is Active">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxCheckBox ID="chkIsActive" runat="server"></dx:ASPxCheckBox>
                                            </td>
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
                                            <td>&nbsp;
                                            </td>
                                            <td class="auto-style8">
                                                <dx:ASPxButton ID="surveyBtn" runat="server" Text="" AutoPostBack="false" ValidationGroup="save">
                                                    <ClientSideEvents Click="SubmitForm"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="right">
                                    <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="BulletedList" ValidationGroup="ErrorGroup" ForeColor="#FF3300">
                                    </dx:ASPxValidationSummary>
                                </div>
                            </asp:Panel>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

                <asp:HiddenField ID="hdWhichButton" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

</asp:Content>
