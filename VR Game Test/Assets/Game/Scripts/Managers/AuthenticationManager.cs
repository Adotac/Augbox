using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Augbox{
public class AuthenticationManager : MonoBehaviour {

    public static async Task<bool> LoginAnonymously() {
        try{
            await Authentication.Login();
            return true;
        }
        catch(Exception e){
            print(e.Message);
            return false;
        }
    }
}
}