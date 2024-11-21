using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherLockOn : MonoBehaviour
{
    float projectileSpeed = 100; // or whatever
    float accuracy = 10;
    
    Vector3 BallisticVelocity(Vector3 source, Vector3 target, Vector3 targetVelocity)
    {
    	// use a few iterations of this recursive function to zero in on 
    	// where the target will be, when the projectile gets there
    	Vector3 horiz = new Vector3(target.x - source.x, 0, target.z - source.z);
    	float t = horiz.magnitude / projectileSpeed;
    	for (int a = 0; a < accuracy; a++)
    	{
    		horiz = new Vector3(target.x + targetVelocity.x * t - source.x, 0, target.z + targetVelocity.z * t - source.z);
    		t = horiz.magnitude / projectileSpeed;
    	}
    	// after t seconds, the cannonball will reach the horizontal location of the target -
    	// so all we have to do is make sure its 'y' coordinate zeros out right there
    	float gravityY = (.5f * Physics.gravity * t * t).y;
    	// now we've calculated how much the projectile will fall during that time
    	// so let's add a 'y' component to the velocity that will take care of the rest
    	float yComponent = (target.y - source.y - gravityY) / t + targetVelocity.y;
    
    	horiz = horiz.normalized * projectileSpeed;
    	return new Vector3(horiz.x, yComponent, horiz.z);
    
    }
}
