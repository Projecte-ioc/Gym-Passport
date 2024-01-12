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
public class Events_test {

    @Rule
    public ActivityScenarioRule<MainActivity> mActivityScenarioRule =
            new ActivityScenarioRule<>(MainActivity.class);

    @Test
    public void events_test() {
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
                allOf(withId(R.id.button_events), withText("Events"),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                1),
                        isDisplayed()));
        materialButton2.perform(click());

        ViewInteraction materialButton3 = onView(
                allOf(withId(R.id.button_n_event), withText("Inserir/Modificar Event."),
                        childAtPosition(
                                childAtPosition(
                                        withId(android.R.id.content),
                                        0),
                                3),
                        isDisplayed()));
        materialButton3.perform(click());

        ViewInteraction appCompatEditText3 = onView(
                allOf(withId(R.id.editTextHour),
                        childAtPosition(
                                childAtPosition(
                                        withId(R.id.nestedScrollView),
                                        0),
                                5),
                        isDisplayed()));
        appCompatEditText3.perform(replaceText("15"), closeSoftKeyboard());

        ViewInteraction materialButton4 = onView(
                allOf(withId(R.id.button_insert_event), withText("Crear event."),
                        childAtPosition(
                                childAtPosition(
                                        withId(R.id.nestedScrollView),
                                        0),
                                1),
                        isDisplayed()));
        materialButton4.perform(click());

        ViewInteraction appCompatEditText4 = onView(
                allOf(withId(R.id.editTextNameEvent),
                        childAtPosition(
                                childAtPosition(
                                        withId(R.id.nestedScrollView),
                                        0),
                                6),
                        isDisplayed()));
        appCompatEditText4.perform(replaceText("Dansa"), closeSoftKeyboard());

        ViewInteraction appCompatEditText5 = onView(
                allOf(withId(R.id.editTextDate),
                        childAtPosition(
                                childAtPosition(
                                        withId(R.id.nestedScrollView),
                                        0),
                                4),
                        isDisplayed()));
        appCompatEditText5.perform(replaceText("2024-05-10"), closeSoftKeyboard());

        ViewInteraction appCompatEditText6 = onView(
                allOf(withId(R.id.editTextAtt),
                        childAtPosition(
                                childAtPosition(
                                        withId(R.id.nestedScrollView),
                                        0),
                                7),
                        isDisplayed()));
        appCompatEditText6.perform(replaceText("8"), closeSoftKeyboard());

        ViewInteraction appCompatEditText7 = onView(
                allOf(withId(R.id.editTextLoc),
                        childAtPosition(
                                childAtPosition(
                                        withId(R.id.nestedScrollView),
                                        0),
                                8),
                        isDisplayed()));
        appCompatEditText7.perform(replaceText("Sala Dansa"), closeSoftKeyboard());

        ViewInteraction materialButton5 = onView(
                allOf(withId(R.id.button_insert_event), withText("Crear event."),
                        childAtPosition(
                                childAtPosition(
                                        withId(R.id.nestedScrollView),
                                        0),
                                1),
                        isDisplayed()));
        materialButton5.perform(click());

        ViewInteraction materialButton6 = onView(
                allOf(withId(R.id.button_modify_event), withText("Modificar event."),
                        childAtPosition(
                                childAtPosition(
                                        withId(R.id.nestedScrollView),
                                        0),
                                2),
                        isDisplayed()));
        materialButton6.perform(click());
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
