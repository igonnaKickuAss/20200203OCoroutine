using System;
using UnityEngine;

namespace OLiOYouxi.OSystem
{
    /// <summary>
    /// 摄像机柔和的对准角色
    /// </summary>
    public class CameraFather
    {
        #region -- Private Data --
        private Vector3 aimPosition = Vector3.zero;
        private Bounds areaMap;
        private Bounds areaCameraSight;

        private Vector2 m_LerpRate = Vector2.zero;
        private Rigidbody2D m_Player = null;
        private BoxCollider2D m_AreaMap = null;
        private BoxCollider2D m_AreaCameraSight = null;

        #endregion

        #region -- VAR --
        float radiusX = 0;
        float radiusY = 0;

        #endregion

        #region -- Public ShotC --
        /// <summary>
        /// 差值范围
        /// </summary>
        public Vector2 LerpRate
        {
            set => m_LerpRate = value;
        }
        /// <summary>
        /// 角色
        /// </summary>
        public Rigidbody2D Player
        {
            set => m_Player = value;
        }
        /// <summary>
        /// 地图区域
        /// </summary>
        public BoxCollider2D AreaMap
        {
            set => m_AreaMap = value;
        }
        /// <summary>
        /// 摄像机视野区域
        /// </summary>
        public BoxCollider2D AreaCameraSight
        {
            set => m_AreaCameraSight = value;
        }
        #endregion

        #region -- MONO APIMethods --
        /// <summary>
        /// 在Awake中调用
        /// </summary>
        public void Awake()
        {
            if (m_Player == null) throw new ArgumentNullException(nameof(m_Player));
            else if (m_AreaMap == null) throw new ArgumentNullException(nameof(m_AreaMap));
            else if (m_AreaCameraSight == null) throw new ArgumentNullException(nameof(m_AreaCameraSight));
            else
            {
                //拿到摄像机当前瞄准目标
                aimPosition = m_Player.transform.position;
                //拿到地图区域
                areaMap = m_AreaMap.bounds;
                m_AreaMap.enabled = false;
                //拿到摄像机视野区域
                areaCameraSight = m_AreaCameraSight.bounds;
                m_AreaCameraSight.enabled = false;
                //算出摄像机事视野区域的x半径
                radiusX = areaCameraSight.size.x / 2;
                //算出摄像机事视野区域的y半径
                radiusY = areaCameraSight.size.y / 2;
            }
        }
        /// <summary>
        /// 在LateUpdate中调用
        /// </summary>
        public void LateUpdate() => m_Player.transform.position = aimPosition; //延迟更新瞄准目标
        /// <summary>
        /// 在FixedUpdate中调用
        /// </summary>
        public void FixedUpdate()
        {
            Transform transform = m_Player.transform;
            //x-axis
            if (m_Player.position.x > (areaMap.min.x + radiusX) && m_Player.position.x < (areaMap.max.x - radiusX))
            {
                aimPosition.x = Mathf.Lerp(
                    transform.position.x,
                    m_Player.position.x,
                    Time.deltaTime * m_LerpRate.x
                );
            }
            else if (m_Player.position.x <= (areaMap.min.x + radiusX))
            {
                aimPosition.x = Mathf.Lerp(
                    transform.position.x,
                    areaMap.min.x + radiusX,
                    Time.deltaTime * m_LerpRate.x
                );
            }
            else if (m_Player.position.x >= (areaMap.max.x - radiusX))
            {
                aimPosition.x = Mathf.Lerp(
                    transform.position.x,
                    areaMap.max.x - radiusX,
                    Time.deltaTime * m_LerpRate.x
                );
            }
            //y-axis
            if (m_Player.position.y > (areaMap.min.y + radiusY) && m_Player.position.y < (areaMap.max.y - radiusY))
            {
                aimPosition.y = Mathf.Lerp(
                    transform.position.y,
                    m_Player.position.y,
                    Time.deltaTime * m_LerpRate.y
                );
            }
            else if (m_Player.position.y <= (areaMap.min.y + radiusY))
            {
                aimPosition.y = Mathf.Lerp(
                    transform.position.y,
                    areaMap.min.y + radiusY,
                    Time.deltaTime * m_LerpRate.y
                );
            }
            else if (m_Player.position.y >= (areaMap.max.y - radiusY))
            {
                aimPosition.y = Mathf.Lerp(
                    transform.position.y,
                    areaMap.max.y - radiusY,
                    Time.deltaTime * m_LerpRate.y
                );
            }
            //z-axis
            aimPosition.z = transform.position.z;
        }
        #endregion
    }
}
