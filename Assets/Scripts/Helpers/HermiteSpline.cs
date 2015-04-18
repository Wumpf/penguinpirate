using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Helpers
{
    class HermiteSpline
    {
        public static float TAP_MOVEMENT_SPEED = 0.01f;

        private Vector3 p_0, p_1, m_0, m_1;

        public Vector3 StartPosition { get { return p_0; } }
        public Vector3 StartMovementDirection { get { return m_0; } }
        public Vector3 FinalPosition { get { return p_1; } }
        public Vector3 FinalMovementDirection { get { return m_1; } }

        public HermiteSpline(Vector3 start, Vector3 startDirection, Vector3 end, Vector3 endDirection)
        {
            p_0 = start;
            p_1 = end;
            m_0 = startDirection;
            m_1 = endDirection;
        }

        // Standard spline if no goal direction is given.
        public HermiteSpline(Vector3 start, Vector3 startDirection, Vector3 end)
        {
            p_0 = start;
            p_1 = end;
            m_0 = startDirection;
            m_1 = (end - start).normalized * TAP_MOVEMENT_SPEED;
        }

        public Vector3 EvaluateAt(float t)
        {
            // p(t) = (2t^3-3t^2+1)p_0 + (t^3-2t^2+t)m_0 + (-2t^3+3t^2)p_1 +(t^3-t^2)m_1 
            float t_square = t * t;
            float t_cube = t_square * t;

            return (2 * t_cube - 3 * t_square + 1) * p_0 + (t_cube - 2 * t_square + t) * m_0 + (-2 * t_cube + 3 * t_square) * p_1 + (t_cube - t_square) * m_1;
        }

        public Vector3 DirectionAt(float t)
        {
            // p(t) = (6t^2-6t)p_0 + (3t^2-4t+1)m_0 + (-6t^2+6t)p_1 +(3t^2-2t)m_1 
            float t_square = t * t;
            
            return (6 * t_square - 6 * t) * p_0 + (3 * t_square - 4 * t + 1) * m_0 + (-6 * t_square + 6 * t) * p_1 +(3 * t_square - 2 * t) * m_1;
        }

        public Vector3 GetNearestPosition(Vector3 position)
        {
            float t;
            return GetNearestPosition(position, out t);
        }

        public Vector3 GetNearestPosition(Vector3 position, out float t)
        {
            t = -1F;
            // calculated below
            float nearestSqrDistance = float.MaxValue;
            // calculated below
            Vector3 nearestPosition = Vector3.zero;

            float stepSize = 0.05F;
            for(float progress = 0F; progress <= 1F; progress += stepSize)
            {
                Vector3 splinePosition = EvaluateAt(progress);
                float sqrDistance = Vector3.SqrMagnitude(splinePosition - position);
                if (sqrDistance < nearestSqrDistance)
                {
                    nearestSqrDistance = sqrDistance;
                    nearestPosition = splinePosition;
                    t = progress;
                }
            }
            return nearestPosition;
        }
    }
}
