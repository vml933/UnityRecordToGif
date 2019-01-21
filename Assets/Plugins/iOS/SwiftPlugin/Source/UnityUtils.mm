//
//  UnityUtils.m
//  Unity-iPhone
//
//  Created by Mark on 2019/1/21.
//

#import <Foundation/Foundation.h>

extern "C" void UnityPostMessage(NSString* gameObject, NSString* methodName, NSString* message)
{
    UnitySendMessage([gameObject UTF8String], [methodName UTF8String], [message UTF8String]);
}
