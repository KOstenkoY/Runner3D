using UnityEngine;
using System.Collections;
using Firebase.Database;

public class DataBase : Singleton<DataBase>
{
    private DatabaseReference dbRef;

    private void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveData(string userName, string data)
    {
        dbRef.Child("users").Child(userName).SetValueAsync(data);
    }

    public IEnumerator LoadData(string userName)
    {
        var user = dbRef.Child("users").Child(userName).GetValueAsync();

        yield return new WaitUntil(predicate: () => user.IsCompleted);

        if(user.Exception != null)
        {
            Debug.LogError(user.Exception);
        }
        else if(user.Result == null)
        {
            Debug.Log("Null");
        }
        else
        {
            DataSnapshot dataSnapshot = user.Result;
            Debug.Log(dataSnapshot.Child(userName).Value.ToString());
        }
    }

    //class User
    //{
    //    private string _userName;

    //    private string _countSteps;

    //    public User(string userName, string recordCountSteps)
    //    {
    //        _userName = userName;
    //        _countSteps = recordCountSteps;
    //    }
    //}
}
