using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;

namespace AdsVenture.Presentation.ContentServer.Helpers
{
    public class SiteMapManager
    {
	    public const string TITLE_SEPARATOR = " > ";
	    public static string GetViewTitle(string currentNodeTitle = null)
	    {
            ISiteMapNode node = SiteMaps.Current.CurrentNode;
		    string title = string.Empty;

		    if ((node == null)) {
			    //Si la pagina no está en el sitemap
                return SiteMaps.Current.RootNode.Title;
		    }
		    //Generar un titulo compuesto por los titulos de los nodos en la ascendencia de la pagina actual
		    title = currentNodeTitle != null ? currentNodeTitle : node.Title;
		    node = node.ParentNode;
		    while ((node != null)) {
			    title = node.Title + (title.Length > 0 ? TITLE_SEPARATOR : string.Empty) + title;
			    node = node.ParentNode;
		    }
		    return title;
	    }

	    public static Stack<BreadcrumNode> GetBreadcrumNodes(string currentNodeTitle = null)
	    {
		    var nodes = new Stack<BreadcrumNode>();
            ISiteMapNode node = (ISiteMapNode)SiteMaps.Current.CurrentNode;

		    //Return an empty stack if current node not exists or it is the root node
            if ((SiteMaps.Current.CurrentNode == null))
            {
			    return nodes;
		    }

		    while ((!(node == null))) {
                nodes.Push(new BreadcrumNode(
                    SiteMaps.Current.CurrentNode.Equals(node) && 
                        currentNodeTitle != null ? currentNodeTitle : node.Title, node.Url, 
                    node.Action, node.Controller, 
                    node.Description, 
                    node.Url == SiteMaps.Current.CurrentNode.Url, 
                    node.Clickable));
			    //El próximo nodo a procesar es el padre
			    node = node.ParentNode;
		    }
		    return nodes;
	    }

	    public static List<BreadcrumNode> GetCurrentSiblingsNodes(object routeValues = null)
	    {
		    return GetCurrentSiblingsNodes(node => routeValues);
	    }

	    public static List<BreadcrumNode> GetCurrentSiblingsNodes(Func<ISiteMapNode, object> routeValues = null)
	    {
		    //Return an empty list if current node not exists or it is the root node
            if ((SiteMaps.Current.CurrentNode == null))
            {
			    return new List<BreadcrumNode>();
		    }

		    if ((routeValues == null)) {
			    routeValues = node => new object();
		    }

		    var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return SiteMaps.Current.CurrentNode.ParentNode.ChildNodes.Cast<ISiteMapNode>().Select(node => new BreadcrumNode(node.Title, urlHelper.Action(node.Action, node.Controller, routeValues(node) ?? new object()), node.Action, node.Controller, node.Description, isCurrent: node.Url == SiteMaps.Current.CurrentNode.Url, isClickable: node.Clickable)).ToList();
	    }
    }

    public class BreadcrumNode
    {
	    public string Title { get; set; }
	    public string Url { get; set; }
	    public string Action { get; set; }
	    public string Controller { get; set; }
	    public string Description { get; set; }
	    public bool IsCurrent { get; set; }
	    public bool IsClickable { get; set; }

	    public BreadcrumNode(string title, string url, string action, string controller, string description, bool isCurrent, bool isClickable)
	    {
		    this.Title = title;
		    this.Url = url;
		    this.Action = action;
		    this.Controller = controller;
		    this.Description = description;
		    this.IsCurrent = isCurrent;
		    this.IsClickable = isClickable;
	    }
    }
}