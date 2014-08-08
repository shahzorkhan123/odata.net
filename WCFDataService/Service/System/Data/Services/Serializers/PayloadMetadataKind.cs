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

namespace System.Data.Services.Serializers
{
    /// <summary>
    /// Container class for a set of enumerations for payload metadatada.
    /// </summary>
    internal static class PayloadMetadataKind
    {
        /// <summary>
        /// Enumeration of payload metadata kinds for navigation links.
        /// </summary>
        internal enum Navigation
        {
            /// <summary>
            /// The 'Url' property of a navigation link.
            /// </summary>
            Url,

            /// <summary>
            /// The 'AssociationLinkUrl' property of a navigation link.
            /// </summary>
            AssociationLinkUrl
        }

        /// <summary>
        /// Enumeration of payload metadata kinds for feeds.
        /// </summary>
        internal enum Feed
        {
            /// <summary>
            /// The 'Id' property of a feed.
            /// </summary>
            Id
        }

        /// <summary>
        /// Enumeration of payload metadata kinds for entries.
        /// </summary>
        internal enum Entry
        {
            /// <summary>
            /// The 'Id' property of an entry.
            /// </summary>
            Id,

            /// <summary>
            /// The 'TypeName' property of an entry.
            /// </summary>
            TypeName,

            /// <summary>
            /// The 'EditLink' property of an entry.
            /// </summary>
            EditLink,

            /// <summary>
            /// The 'ETag' property of an entry.
            /// </summary>
            ETag,
        }

        /// <summary>
        /// Enumeration of payload metadata kinds for association links.
        /// </summary>
        internal enum Association
        {
            /// <summary>
            /// The 'Url' property of an association link.
            /// </summary>
            Url,
        }

        /// <summary>
        /// Enumeration of payload metadata kinds for streams.
        /// </summary>
        internal enum Stream
        {
            /// <summary>
            /// The 'EditLink' property of a stream.
            /// </summary>
            EditLink,

            /// <summary>
            /// The 'ReadLink' property of a stream.
            /// </summary>
            ReadLink,

            /// <summary>
            /// The 'ContentType' property of a stream.
            /// </summary>
            ContentType,

            /// <summary>
            /// The 'ETag' property of a stream.
            /// </summary>
            ETag
        }

        /// <summary>
        /// Enumeration of payload metadata kinds for actions/functions.
        /// </summary>
        internal enum Operation
        {
            /// <summary>
            /// The 'Title' property of an operation.
            /// </summary>
            Title,

            /// <summary>
            /// The 'Target' property of an operation.
            /// </summary>
            Target
        }
    }
}