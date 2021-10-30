<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListConfigs.aspx.cs" Inherits="SPEventReceiverNotificationsLayouts.Layouts.SPEventReceiverNotificationsLayouts.ListConfigs" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br/>
    <asp:Label ID="InfoLabel1" runat="server" Font-Size="Large"></asp:Label>
    <h2>Описание механизма</h2>
    Немедленные оповещения при изменении, добавлении элементов списка.
    <br/>
    Типы оповещений:
    <ol>
        <li>Изменение элемента</li>
        <li>Добавление элемента в список</li>
        <li>Добавление вложения</li>
    </ol>
    <h3>Common settings</h3>
    <div style="margin-top: 10px;margin-bottom: 10px;">
        <asp:HyperLink ID="SiteConfigsLink" runat="server" Text="Common configs"></asp:HyperLink>
    </div>
    <br/>
    <SharePoint:SPGridView ID="ConfigsTable" runat="server" AutoGenerateColumns="false">
        <RowStyle BackColor="#f6f7f8" Height="30px" HorizontalAlign="Left" />
        <AlternatingRowStyle BackColor="White" ForeColor="#000" Height="30px" HorizontalAlign="Left" />
        <HeaderStyle Font-Bold="true" HorizontalAlign="Left" CssClass="ms-viewheadertr" />
        <Columns>
            <asp:HyperLinkField ControlStyle-CssClass="table-links" DataTextField="ConfName" DataNavigateUrlFields="ConfUrl" 
                DataNavigateUrlFormatString="{0}" ItemStyle-Width = "500" HeaderText="Config" />
            <asp:BoundField DataField="ConfEnabled" HeaderText="Enabled" ItemStyle-Width = "190" />
            <asp:BoundField DataField="ConfModifiedDate" HeaderText="ModifiedDate" ItemStyle-Width = "190" />
        </Columns>
    </SharePoint:SPGridView>
    <div style="margin-top: 10px;">
        <asp:Button ID="ButtonAdd" runat="server" Text="Add" OnClick="ButtonAdd_EventHandler"/>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Configs List
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
    <a id="SettingsLink" runat="server">Settings</a>
    <span>
        <span style="height:16px;width:16px;position:relative;display:inline-block;overflow:hidden;">
            <img src="/_layouts/15/images/spcommon.png?rev=43#ThemeKey=spcommon" alt=":" style="position:absolute;left:-109px !important;top:-232px !important;"/>
        </span>
    </span>
    <span>Items Notifications</span>
</asp:Content>