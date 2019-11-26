﻿<%@ Control Language="vb" ClassName="DateWidget" %>
<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<script runat="server">
	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		DateLabel.Text = New DateTime(DateTime.Now.Year, 3, 14).ToLongDateString()
	End Sub

</script>
<style type="text/css">

.dxeBase
{
	font: 12px Tahoma;
}
</style>

<p>
	<dx:ASPxLabel runat="server" ID="DateLabel" ClientInstanceName="dateLabel" 
        Font-Size="Large">
	</dx:ASPxLabel>
</p>

