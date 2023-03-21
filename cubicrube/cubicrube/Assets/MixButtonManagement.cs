using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class MixButtonManagement : MonoBehaviour
    {
        public GameObject go;

        public void MixButtonPress()
        {
            go.GetComponent<CubeTurning>().MixButtonPress();
        }
    }
}
