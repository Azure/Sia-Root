using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Protocol
{
    public class LinksForSerialization
    {
        public Metadata Metadata { get; set; }
        public LinksCollection Links { get; set; }
    }

    public class Metadata
    {
        public PaginationMetadataRecord Pagination { get; set; }
    }

    public class PaginationMetadataRecord
    {
        public string PageNumber { get; set; }
        public string PageSize { get; set; }
        public string TotalRecords { get; set; }
        public string TotalPages { get; set; }
    }

    public class LinksCollection
    {
        public OperationLinks Operations { get; set; }
        public PaginationLinks Pagination { get; set; }
        public RelationLinks Related { get; set; }
    }

    public class OperationLinks
    {
        public SingleOperationLinks Single { get; set; }
        public MultipleOperationLinks Multiple { get; set; }
    }

    public class SingleOperationLinks
    {
        public string Get { get; set; }
        public string Post { get; set; }
        public string Put { get; set; }
    }

    public class MultipleOperationLinks
    {
        public string Get { get; set; }
    }

    public class PaginationLinks
    {
        public string Next { get; set; }
        public string Previous { get; set; }
    }

    public class RelationLinks
    {
        public RelatedChildLinks Children { get; set; }
        public RelatedParentLinks Parent { get; set; }
    }

    public class RelatedChildLinks
    {
        public string Events { get; set; }
    }

    public class RelatedParentLinks
    {
        public string Incident { get; set; }
    }
}
