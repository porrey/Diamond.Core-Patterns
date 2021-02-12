using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diamond.Core.AspNet.Swagger {
	/// <summary>
	/// 
	/// </summary>
	public class JsonPatchDocumentFilter : IDocumentFilter {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="swaggerDoc"></param>
		/// <param name="context"></param>
		public virtual void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) {
			//
			// Remove the schemas.
			//
			this.RemoveSchema(swaggerDoc, "IContractResolver");
			this.RemoveSchema(swaggerDoc, "OperationType");
			this.RemoveSchema(swaggerDoc, "JsonPatchDocument");
			this.RemoveSchema(swaggerDoc, "Operation");

			//
			// Add the JSON patch references.
			//
			this.AddJsonPatchTypes(swaggerDoc);

			//
			// Fix up the patch references
			//
			IEnumerable<KeyValuePair<OperationType, OpenApiOperation>> pathItems = swaggerDoc.Paths.SelectMany(p => p.Value.Operations).Where(p => p.Key == OperationType.Patch);

			//
			// Point any patch operations to the JsonPatchDocument reference.
			//
			if (pathItems.Any()) {
				foreach (KeyValuePair<OperationType, OpenApiOperation> path in pathItems) {
					if (path.Value.RequestBody != null) {
						IEnumerable<KeyValuePair<string, OpenApiMediaType>> bodyItems = path.Value.RequestBody.Content.Where(c => c.Key != "application/json-patch+json");

						if (bodyItems.Any()) {
							foreach (KeyValuePair<string, OpenApiMediaType> item in bodyItems) {
								if (item.Value.Schema.Reference.ReferenceV2.Contains("JsonPatchDocument") ||
									item.Value.Schema.Reference.ReferenceV3.Contains("JsonPatchDocument")) {
									item.Value.Schema = new OpenApiSchema {
										Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "JsonPatchDocument" }
									};
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Removes items where the key passed mactehs any part of the schema key.
		/// </summary>
		/// <param name="swaggerDoc">A reference to the Swagger document.</param>
		/// <param name="key">The key or partial ket ot match.</param>
		protected virtual void RemoveSchema(OpenApiDocument swaggerDoc, string key) {
			//
			// Get a list of schema keys matching the key passed.
			//
			IEnumerable<string> items = (from tbl in swaggerDoc.Components.Schemas.Keys
										 where tbl.Contains(key)
										 select tbl).ToArray();

			//
			// Check for matches.
			//
			if (items.Any()) {
				//
				// Delete the matchers.
				//
				foreach (var item in items) {
					swaggerDoc.Components.Schemas.Remove(item);
				}
			}
		}

		/// <summary>
		/// Adss the JSON patch definitions.
		/// </summary>
		/// <param name="swaggerDoc">A reference to the Swagger document.</param>
		protected virtual void AddJsonPatchTypes(OpenApiDocument swaggerDoc) {
			//
			// Add the Operation object.
			//
			swaggerDoc.Components.Schemas.Add("Operation", new OpenApiSchema {
				Type = "object",
				Title = "Operation",
				Properties = new Dictionary<string, OpenApiSchema>
				{
					{"op", new OpenApiSchema{ Type = "string", Description = $"Specifies one of the following operations for the model property: { String.Join(", ", Enum.GetNames(typeof(Microsoft.AspNetCore.JsonPatch.Operations.OperationType)))}" } },
					{"value", new OpenApiSchema{ Type = "object", Description = "The value to apply for the model property." , Nullable = true } },
					{"path", new OpenApiSchema{ Type = "string", Description = "The path to the model property to apply the operation and value."  } }
				},
				Description = "Operation to perform on the model property."
			});

			//
			// Add the JsonPatchDocument object.
			//
			swaggerDoc.Components.Schemas.Add("JsonPatchDocument", new OpenApiSchema {
				Type = "array",
				Items = new OpenApiSchema {
					Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "Operation" }
				},
				Description = "Array of patch operations to perform on the model."
			});
		}
	}
}