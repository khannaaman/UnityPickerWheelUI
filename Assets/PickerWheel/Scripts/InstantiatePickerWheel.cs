using UnityEngine ;
using UnityEngine.UI ;
using DG.Tweening ;
using UnityEngine.Events ;
using System.Collections.Generic ;

namespace EasyUI.PickerWheelUI {

   public class InstantiatePickerWheel : MonoBehaviour {

        public GameObject instance;

      private void Start () {
            Instantiate(instance, new Vector3(200, 200, 200), Quaternion.identity);
        }
   }
}