//   OData .NET Libraries ver. 5.6.2
//   Copyright (c) Microsoft Corporation. All rights reserved.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

namespace System.Data.Services.Parsing
{
    #region Namespaces
    using System;
    using System.Data.Services.Providers;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Spatial;
    using Microsoft.Data.OData.Query.SemanticAst;
    using DataServiceProviderMethods = System.Data.Services.Providers.DataServiceProviderMethods;
    using OpenTypeMethods = System.Data.Services.Providers.OpenTypeMethods;
    using Strings = System.Data.Services.Strings;
    #endregion Namespaces

    /// <summary>
    /// This class provides static methods to parse query options and compose 
    /// them on an existing query.
    /// </summary>
    internal static class RequestQueryParser
    {
        #region Internal methods.

        /// <summary>Sorts a query like a SQL ORDER BY clause does.</summary>
        /// <param name="source">Original source for query.</param>
        /// <param name="orderingInfo">Ordering definition to compose.</param>
        /// <returns>The composed query.</returns>
        internal static Expression OrderBy(Expression source, OrderingInfo orderingInfo)
        {
            Debug.Assert(source != null, "source != null");
            Debug.Assert(orderingInfo != null, "orderingInfo != null");

            Expression queryExpr = source;
            bool useOrderBy = true;
            foreach (OrderingExpression o in orderingInfo.OrderingExpressions)
            {
                LambdaExpression selectorLambda = (LambdaExpression)o.Expression;

                Type selectorType = selectorLambda.Body.Type;
                Debug.Assert(selectorType != null, "type != null");
                
                // ensure either the expression type is orderable (ie, primitive) or its an open expression.
                if (!WebUtil.IsPrimitiveType(selectorType) && !OpenTypeMethods.IsOpenExpression(selectorLambda.Body))
                {
                    throw DataServiceException.CreateBadRequestError(Strings.RequestQueryParser_OrderByDoesNotSupportType(WebUtil.GetTypeName(selectorType)));
                }

                if (useOrderBy)
                {
                    queryExpr = o.IsAscending ? queryExpr.QueryableOrderBy(selectorLambda) : queryExpr.QueryableOrderByDescending(selectorLambda);
                }
                else
                {
                    queryExpr = o.IsAscending ? queryExpr.QueryableThenBy(selectorLambda) : queryExpr.QueryableThenByDescending(selectorLambda);
                }

                useOrderBy = false;
            }

            return queryExpr;
        }

        /// <summary>Filters a query like a SQL WHERE clause does.</summary>
        /// <param name="service">Service with data and configuration.</param>
        /// <param name="requestDescription">RequestDescription instance containing information about the current request being parsed.</param>
        /// <param name="source">Original source for query expression.</param>
        /// <param name="predicate">Predicate to compose.</param>
        /// <returns>The composed query expression.</returns>
        internal static Expression Where(IDataService service, RequestDescription requestDescription, Expression source, string predicate)
        {
            Debug.Assert(service != null, "service != null");
            Debug.Assert(source != null, "source != null");
            Debug.Assert(predicate != null, "predicate != null");
            Debug.Assert(requestDescription != null, "requestDescription != null");

            FilterClause filterClause = new RequestExpressionParser(service, requestDescription, predicate).ParseFilter();

            Type queryElementType = source.ElementType();
            Debug.Assert(queryElementType != null, "queryElementType != null");

            ParameterExpression parameterForIt = Expression.Parameter(queryElementType, "it");
            Debug.Assert(
                (requestDescription.TargetResourceSet == null && (requestDescription.TargetResourceType == null || requestDescription.TargetResourceType.ResourceTypeKind != ResourceTypeKind.EntityType)) ||
                (requestDescription.TargetResourceType != null && requestDescription.TargetResourceType.ResourceTypeKind == ResourceTypeKind.EntityType),
                "setForIt cannot be null if typeForIt is an entity type.");
            Debug.Assert(
                requestDescription.TargetResourceType == null && parameterForIt.Type == typeof(object) ||
                requestDescription.TargetResourceType != null && requestDescription.TargetResourceType.InstanceType == parameterForIt.Type,
                "non-open type expressions should have a typeForIt");

            var translator = NodeToExpressionTranslator.Create(service, requestDescription, parameterForIt);
            LambdaExpression lambda = translator.TranslateFilterClause(filterClause);
            return source.QueryableWhere(lambda);
        }

        #endregion Internal methods.
    }
}