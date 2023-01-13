
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <CoreLocation/CoreLocation.h>
#include "UnityFramework/UnityFramework-Swift.h"

extern UIViewController *UnityGetGLViewController();

@interface iOSPlugin : NSObject


@end


@implementation iOSPlugin

+(void)alertView:(NSString *)title addMessage:(NSString *) message {
    UIAlertController *alert = [UIAlertController alertControllerWithTitle:title
                                message:message
                                preferredStyle:UIAlertControllerStyleAlert];

    UIAlertAction *defaultAction = [UIAlertAction actionWithTitle:@"OK" style:UIAlertActionStyleDefault
        handler:^(UIAlertAction *action){}];

    [alert addAction:defaultAction];
    [self presentViewController:alert animated:YES completion:nil];
}


@end

extern "C" {
    
    void _ShowAlert(const chat *title, const char *message){
        [iOSPlugin [alertView:title, message]];
    }
}
