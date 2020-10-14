using System;
using JetBrains.Annotations;

namespace AtlasStudio.Models
{
    public class BlockDescription
    {
        public BlockDescription(string name, [NotNull] BlockFace top, [NotNull] BlockFace left, [NotNull] BlockFace right)
        {
            Name = name;
            Top = top ?? throw new ArgumentNullException(nameof(top));
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public string Name { get; }

        public BlockFace Top { get; }

        public BlockFace Left { get; }

        public BlockFace Right { get; }
    }
}
