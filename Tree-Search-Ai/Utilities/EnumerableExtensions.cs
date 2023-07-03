namespace Ai.Utilities
{
    internal static class EnumerableExtensions
    {
        internal static T GetRandom<T>(this IEnumerable<T> source, Random randomGenerator, int totalCount = -1)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (randomGenerator == null) { throw new ArgumentNullException(nameof(randomGenerator)); }

            if (totalCount < 0)
            {
                return source.Shuffle(randomGenerator).First();
            }
            else
            {
                return source.Skip(randomGenerator.Next(totalCount)).First();
            }
        }

        internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random randomGenerator)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (randomGenerator == null) { throw new ArgumentNullException(nameof(randomGenerator)); }

            List<T> buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = randomGenerator.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}