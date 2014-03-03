﻿/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings

using Ironman.Core.ActionFilters;
using Ironman.Core.BaseClasses;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Utilities.DataTypes;
using Utilities.IO;

#endregion Usings

namespace Ironman.Core.API.BaseClasses
{
    /// <summary>
    /// API Controller base class
    /// </summary>
    [Compress(Minify = false)]
    public abstract class APIControllerBaseClass : Ironman.Core.BaseClasses.ControllerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected APIControllerBaseClass()
            : base()
        {
        }

        /// <summary>
        /// API manager class
        /// </summary>
        protected static Manager.Manager APIManager { get { return Utilities.IoC.Manager.Bootstrapper.Resolve<Manager.Manager>(); } }

        /// <summary>
        /// Version number for this API endpoint
        /// </summary>
        protected abstract int Version { get; }

        /// <summary>
        /// Gets all items of the specified type
        /// GET: {APIRoot}/{ModelName}/
        /// </summary>
        /// <param name="ModelName">Model name</param>
        /// <returns>The resulting list of items</returns>
        [HttpGet]
        public ActionResult All(string ModelName)
        {
            return Serialize<IEnumerable<Dynamo>>(APIManager.All(Version, ModelName, Request.QueryString.Get("Embedded").Check("").Split(',')));
        }

        /// <summary>
        /// Gets the item of the specified type, with the specified ID
        /// GET: {APIRoot}/{ModelName}/{ID}
        /// </summary>
        /// <param name="ModelName">Model name</param>
        /// <param name="ID">ID of the object to get</param>
        /// <returns>The resulting item</returns>
        [HttpGet]
        public ActionResult Any(string ModelName, string ID)
        {
            return Serialize<Dynamo>(APIManager.Any(Version, ModelName, ID, Request.QueryString.Get("Embedded").Check("").Split(',')));
        }

        /// <summary>
        /// Deletes the specified object
        /// </summary>
        /// <param name="ModelName">Model name</param>
        /// <param name="ID">ID of the object to delete</param>
        /// <returns>The result</returns>
        [HttpDelete]
        public ActionResult Delete(string ModelName, string ID)
        {
            return Serialize<Dynamo>(APIManager.Delete(Version, ModelName, ID));
        }

        /// <summary>
        /// Gets the property specified of the item of the specified type, with the specified ID
        /// GET: {APIRoot}/{ModelName}/{ID}/{PropertyName}
        /// </summary>
        /// <param name="ModelName">Model name</param>
        /// <param name="ID">ID of the object to get</param>
        /// <param name="PropertyName">Property name</param>
        /// <returns>The resulting item</returns>
        [HttpGet]
        public ActionResult GetProperty(string ModelName, string ID, string PropertyName)
        {
            return Serialize(APIManager.GetProperty(Version, ModelName, ID, PropertyName, Request.QueryString.Get("Embedded").Check("").Split(',')));
        }

        /// <summary>
        /// Saves the specified object
        /// </summary>
        /// <param name="ModelName">Model name</param>
        /// <param name="Model">Model to save</param>
        /// <returns>The result</returns>
        [AcceptVerbs(HttpVerbs.Put | HttpVerbs.Post | HttpVerbs.Patch)]
        [DeserializationFilter(ObjectType = typeof(IEnumerable<ExpandoObject>), Parameter = "Model")]
        public ActionResult Save(string ModelName, IEnumerable<ExpandoObject> Model)
        {
            return Serialize<Dynamo>(APIManager.Save(Version, ModelName, Model.Select(x => new Dynamo(x))));
        }
    }
}