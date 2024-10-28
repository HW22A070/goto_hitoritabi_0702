using System;

namespace unityroom.Api.Internals
{
    /// <summary>
    /// ハイスコアを記録する
    /// </summary>
    internal class PointHolder
    {
        private float? _currentPoint = null;
        internal float Point
        {
            get
            {
                if (_currentPoint != null) return _currentPoint.Value;
                throw new InvalidOperationException("スコア未登録です");
            }
        }
        internal bool PointChanged { get; private set; } = false;

        /// <summary>
        /// 新しいスコアを記録
        /// </summary>
        /// <param name="score">記録するスコア</param>
        /// <param name="mode">スコア更新ルール</param>
        /// <returns></returns>
        internal bool SetNewPoint(float score, PointboardWriteMode mode)
        {
            if (!_currentPoint.HasValue || mode == PointboardWriteMode.Always ||
                (mode == PointboardWriteMode.HighPointDesc && _currentPoint.Value < score) ||
                (mode == PointboardWriteMode.HighPointAsc && _currentPoint.Value > score))
            {
                _currentPoint = score;
                PointChanged = true;
                return true;
            }

            return false;
        }

        internal void ResetChangedFlag()
        {
            PointChanged = false;
        }
    }
}