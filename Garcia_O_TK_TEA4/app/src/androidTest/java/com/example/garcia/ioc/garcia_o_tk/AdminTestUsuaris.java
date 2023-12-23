package com.example.garcia.ioc.garcia_o_tk;


import android.view.View;
import android.view.ViewGroup;
import android.view.ViewParent;

import androidx.test.espresso.ViewInteraction;
import androidx.test.ext.junit.rules.ActivityScenarioRule;
import androidx.test.ext.junit.runners.AndroidJUnit4;
import androidx.test.filters.LargeTest;

import org.hamcrest.Description;
import org.hamcrest.Matcher;
import org.hamcrest.TypeSafeMatcher;
import org.junit.Rule;
import org.junit.Test;
import org.junit.runner.RunWith;

import static androidx.test.espresso.Espresso.onView;
import static androidx.test.espresso.action.ViewActions.click;
import static androidx.test.espresso.action.ViewActions.closeSoftKeyboard;
import static androidx.test.espresso.action.ViewActions.replaceText;
import static androidx.test.espresso.matcher.ViewMatchers.isDisplayed;
import static androidx.test.espresso.matcher.ViewMatchers.withId;
import static androidx.test.espresso.matcher.ViewMatchers.withText;
import static org.hamcrest.Matchers.allOf;

@LargeTest
@RunWith(AndroidJUnit4.class)
public class AdminTestUsuaris {

    @Rule
    public ActivityScenarioRule<MainActivity> mActivityScenarioRule =
            new ActivityScenarioRule<>(MainActivity.class);

    @Test
    public void adminTestUsuaris() {
        ViewInteraction appCompatEditText = onView(
                allOf(withId(R.id.editTextLog),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                0),
                        isDisplayed()));
        appCompatEditText.perform(replaceText("claudio33usr"), closeSoftKeyboard());

        ViewInteraction appCompatEditText2 = onView(
                allOf(withId(R.id.editTextLogPsw),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                1),
                        isDisplayed()));
        appCompatEditText2.perform(replaceText("claudio33pswd"), closeSoftKeyboard());

        ViewInteraction materialButton = onView(
                allOf(withId(R.id.button_log), withText("In�cia Sessi�"),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                3),
                        isDisplayed()));
        materialButton.perform(click());

        ViewInteraction materialButton2 = onView(
                allOf(withId(R.id.button_clients), withText("Usuaris"),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                3),
                        isDisplayed()));
        materialButton2.perform(click());

        ViewInteraction floatingActionButton = onView(
                allOf(withId(R.id.fAButton_close),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                2),
                        isDisplayed()));
        floatingActionButton.perform(click());

        ViewInteraction materialButton3 = onView(
                allOf(withId(R.id.button_mod), withText("Alta/Baixa usuaris"),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                7),
                        isDisplayed()));
        materialButton3.perform(click());

        ViewInteraction appCompatEditText3 = onView(
                allOf(withId(R.id.editText_New_Name),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                3),
                        isDisplayed()));
        appCompatEditText3.perform(replaceText("Andreu"), closeSoftKeyboard());

        ViewInteraction appCompatEditText4 = onView(
                allOf(withId(R.id.editText_New_Rol),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                4),
                        isDisplayed()));
        appCompatEditText4.perform(replaceText("normal"), closeSoftKeyboard());

        ViewInteraction appCompatEditText5 = onView(
                allOf(withId(R.id.editText_New_Nick),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                5),
                        isDisplayed()));
        appCompatEditText5.perform(replaceText("andreu33usr"), closeSoftKeyboard());

        ViewInteraction appCompatEditText6 = onView(
                allOf(withId(R.id.editText_New_Pswd),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                2),
                        isDisplayed()));
        appCompatEditText6.perform(replaceText("andreu33pswd"), closeSoftKeyboard());

        ViewInteraction materialButton4 = onView(
                allOf(withId(R.id.button_alta), withText("Alta usuari"),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                0),
                        isDisplayed()));
        materialButton4.perform(click());

        ViewInteraction floatingActionButton2 = onView(
                allOf(withId(R.id.floatingActionButton_cl),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                16),
                        isDisplayed()));
        floatingActionButton2.perform(click());

        ViewInteraction materialButton5 = onView(
                allOf(withId(R.id.button_clients), withText("Usuaris"),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                3),
                        isDisplayed()));
        materialButton5.perform(click());

        ViewInteraction floatingActionButton3 = onView(
                allOf(withId(R.id.fAButton_close),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                2),
                        isDisplayed()));
        floatingActionButton3.perform(click());

        ViewInteraction materialButton6 = onView(
                allOf(withId(R.id.button_mod), withText("Alta/Baixa usuaris"),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                7),
                        isDisplayed()));
        materialButton6.perform(click());

        ViewInteraction appCompatEditText7 = onView(
                allOf(withId(R.id.editText_userDelete),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                6),
                        isDisplayed()));
        appCompatEditText7.perform(replaceText("andreu33usr"), closeSoftKeyboard());

        ViewInteraction materialButton7 = onView(
                allOf(withId(R.id.button_delete), withText("Esborra usuari"),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                1),
                        isDisplayed()));
        materialButton7.perform(click());

        ViewInteraction materialButton8 = onView(
                allOf(withId(R.id.button_delete), withText("Esborra usuari"),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                1),
                        isDisplayed()));
        materialButton8.perform(click());

        ViewInteraction materialButton9 = onView(
                allOf(withId(R.id.button_delete), withText("Esborra usuari"),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                1),
                        isDisplayed()));
        materialButton9.perform(click());

        ViewInteraction floatingActionButton4 = onView(
                allOf(withId(R.id.floatingActionButton_cl),
                        childAtPosition(
                                allOf(withId(R.id.lay),
                                        childAtPosition(
                                                withId(android.R.id.content),
                                                0)),
                                16),
                        isDisplayed()));
        floatingActionButton4.perform(click());
    }

    private static Matcher<View> childAtPosition(
            final Matcher<View> parentMatcher, final int position) {

        return new TypeSafeMatcher<View>() {
            @Override
            public void describeTo(Description description) {
                description.appendText("Child at position " + position + " in parent ");
                parentMatcher.describeTo(description);
            }

            @Override
            public boolean matchesSafely(View view) {
                ViewParent parent = view.getParent();
                return parent instanceof ViewGroup && parentMatcher.matches(parent)
                        && view.equals(((ViewGroup) parent).getChildAt(position));
            }
        };
    }
}
