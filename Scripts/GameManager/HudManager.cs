using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.GameManager
{
    class HudManager : Manager<HudManager>
    {
    
        [Header("Texts")]       
        [SerializeField] private Text m_TxtNode;

        protected override IEnumerator InitCoroutine()
        {
            yield break;
        }

        protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
        {
            m_TxtNode.text = e.eNode.ToString();
        }
    }
}
