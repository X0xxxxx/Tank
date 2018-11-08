using System;
using UnityEngine;
using System.Collections;
/*正态分布函数（待用）*/
public class GaussianRNG : MonoBehaviour {

    public void print()
    {
        Debug.Log(fx(0, 1, 0));
        Debug.Log(selfCaculate(0.4f));
    }


	public static float selfCaculate(float u){
		float ret = 0;
		if (u < -3.89) {  
			return 0;  
		} else if (u > 3.89) {  
			return 1;  
		}  
		float temp = -3.89f;  //Y的最大值
		while (temp <= u) {  
			ret += 0.0001f * fx (temp, 1, 0);  
			temp += 0.0001f;  
		}  
		return ret;  
	}//返回积分


	private static float fx(float x, float q, float u){  
		float ret = 0;  
		double d =  Math.Sqrt(Math.PI * 2) * q;  
		double exp = Math.Pow(Math.E ,- Math.Pow (x - u, 2) / (2 * Math.Pow(q, 2)));
		double y = exp / d;
		ret = (float) y;  
		return ret;  
	}  //返回Y
}
