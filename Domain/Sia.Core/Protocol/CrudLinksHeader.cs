namespace Sia.Core.Protocol
{
    using Sia.Core.Validation;

    public class CrudLinksHeader : ILinksHeader
    {
        private OperationLinks OperationLinks { get; }
        private RelationLinks RelationLinks { get; }

        public CrudLinksHeader(OperationLinks operationLinks, RelationLinks relationLinks)
        {
            OperationLinks = ThrowIf.Null(operationLinks, nameof(operationLinks));
            RelationLinks = relationLinks;
        }

        public LinksForSerialization GetHeaderValues() =>
            new LinksForSerialization()
            {
                Links = new LinksCollection()
                {
                    Operations = OperationLinks,
                    Related = RelationLinks
                }
            };
    }
}
