using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;

using Microsoft.AspNetCore.Components;

namespace BlazorCRUD.Client.Shared
{
    public partial class Modal
    {
        [Parameter, EditorRequired]
        public RenderFragment? ChildContent { get; set; }

        [Parameter, EditorRequired]
        public Action? Close { get; set; }

        [Parameter]
        public string Style { get; set; } = "";
    }
}