
#import <Foundation/Foundation.h>
#include "UnityFramework/UnityFramework-Swift.h"

extern "C" {
    
#pragma mark - Functions
    
    NSString* _addTwoNumberInIOS(int a , int b) {
       
        NSString result = [[UnityPlugin shared] AddTwoNumberWithA:(a) b:(b)];
        return result;
    }

    void _start {
        [[UnityPlugin shared] Start];
    }
}
